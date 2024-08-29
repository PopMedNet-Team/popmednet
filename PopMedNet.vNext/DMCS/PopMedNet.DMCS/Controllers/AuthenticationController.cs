using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Data.Model;
using PopMedNet.DMCS.PMNApi;
using PopMedNet.DMCS.PMNApi.DTO;
using Serilog;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseApiController
    {
        readonly SignInManager<Data.Identity.IdentityUser> _signInManager;
        readonly IOptions<DMCSConfiguration> _config;
        readonly Services.BackgroundWorkerQueue _backgroundWorkerQueue;

        public AuthenticationController(ModelContext modelDb, 
            SignInManager<Data.Identity.IdentityUser> signInManager, 
            ILogger logger, 
            IOptions<DMCSConfiguration> config,
            Services.BackgroundWorkerQueue backgroundWorkerQueue)
            : base(modelDb, logger.ForContext("SourceContext", typeof(AuthenticationController).FullName))
        {
            this._signInManager = signInManager;
            this._config = config;
            this._backgroundWorkerQueue = backgroundWorkerQueue;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthenticationRequest auth)
        {
            if (string.IsNullOrEmpty(auth.username))
            {
                ModelState.AddModelError("username", "Username is required.");
            }

            if (string.IsNullOrEmpty(auth.password))
            {
                ModelState.AddModelError("password", "Password is required.");
            }

            if (ModelState.ErrorCount > 0)
            {
                this.logger.Information("Authentication failed for user: {0}", string.IsNullOrEmpty(auth.username) ? "<unknown>" : auth.username);
                return BadRequest(ModelState);
            }

            using (var pmnClient = new PMNApiClient(_config.Value.PopMedNet.ApiServiceURL, _config.Value.PopMedNet.ServiceUserCredentials.UserName, _config.Value.PopMedNet.ServiceUserCredentials.GetPassword()))
            {
                try
                {
                    var validatedUser = await pmnClient.ValidateUserLogin(new LoginDTO { UserName = auth.username, Password = auth.password, Enviorment = "DMCS", IPAddress = HttpContext.Connection.RemoteIpAddress.ToString() });

                    if (validatedUser == null)
                    {
                        ModelState.AddModelError("error", "Authentication failed, please reconfirm your username and password.");
                        return BadRequest(ModelState);
                    }
                    
                    var identityUser = await _signInManager.UserManager.FindByIdAsync(validatedUser.ID.Value.ToString());
                    if (identityUser == null)
                    {
                        var result = await _signInManager.UserManager.CreateAsync(new Data.Identity.IdentityUser { Id = validatedUser.ID.Value, UserName = validatedUser.UserName, Email = validatedUser.Email }, auth.password);
                        identityUser = await _signInManager.UserManager.FindByIdAsync(validatedUser.ID.Value.ToString());
                    }
                    else if(!validatedUser.UserName.Equals(identityUser.UserName) || !validatedUser.Email.Equals(identityUser.Email))
                    {
                        //NOTE: any changes to the identity user must be done using the UserManager!

                        IdentityResult result;
                        if (!validatedUser.UserName.Equals(identityUser.UserName))
                        {
                            result = await _signInManager.UserManager.SetUserNameAsync(identityUser, validatedUser.UserName);
                            if (result.Succeeded == false)
                            {
                                var errors = result.Errors.Select(e => string.Format("{0} - {1}", e.Code, e.Description)).ToArray();
                                this.logger.Error("Error updating username for the authentication identity: " + string.Join(Environment.NewLine, errors));

                                foreach (var err in result.Errors)
                                {
                                    ModelState.AddModelError("error", err.Description);
                                }
                                return BadRequest(ModelState);
                            }
                        }

                        if (!validatedUser.Email.Equals(identityUser.Email))
                        {
                            result = await _signInManager.UserManager.SetEmailAsync(identityUser, validatedUser.Email);
                            if (result.Succeeded == false)
                            {
                                var errors = result.Errors.Select(e => string.Format("{0} - {1}", e.Code, e.Description)).ToArray();
                                this.logger.Error("Error updating email address for the authentication identity: " + string.Join(Environment.NewLine, errors));

                                foreach (var err in result.Errors)
                                {
                                    ModelState.AddModelError("error", err.Description);
                                }
                                return BadRequest(ModelState);
                            }
                        }

                        identityUser = await _signInManager.UserManager.FindByIdAsync(validatedUser.ID.Value.ToString());

                    }

                    var modelUser = await modelDb.Users.FindAsync(identityUser.Id);
                    if(modelUser == null)
                    {
                        modelUser = new User { ID = identityUser.Id, Email = identityUser.Email, UserName = identityUser.UserName };
                        await modelDb.Users.AddAsync(modelUser);
                        await modelDb.SaveChangesAsync();
                    }else if(!identityUser.UserName.Equals(modelUser.UserName) || !identityUser.Email.Equals(modelUser.Email))
                    {
                        modelUser.UserName = identityUser.UserName;
                        modelUser.Email = identityUser.Email;
                        await modelDb.SaveChangesAsync();
                    }

                    var newUserResult = await _signInManager.PasswordSignInAsync(auth.username, auth.password, false, false);
                    if(newUserResult.Succeeded == false)
                    {
                        //likely the password has been changed in PMN and not synced yet, update here to match the one that worked for PMN
                        await ((Data.Identity.DMCSUserManager)_signInManager.UserManager).UpdatePasswordHash(identityUser, _signInManager.UserManager.PasswordHasher.HashPassword(identityUser, auth.password));
                    }

                    logger.Information("Authentication successful for user: {0}", auth.username);

                    var cook = Crypto.EncryptStringAES(string.Format("{0}:{1}:{2}", auth.username, auth.password, Guid.NewGuid()), _config.Value.Settings.Hash, _config.Value.Settings.Key);

                    var co = new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true 
                    };
                    Response.Cookies.Append("DMCS-User", cook, co);

                    //TODO:move to a background task, add any missing datamarts the user can see, remove association if they cannot any longer
                    var pmnDataMartIDs = await pmnClient.GetDataMarts(validatedUser.ID.Value);

                    var userDataMarts = await this.modelDb.UserDataMarts.Where(x => x.UserID == validatedUser.ID.Value).ToArrayAsync();

                    using (var trx = await modelDb.Database.BeginTransactionAsync())
                    {

                        var newDataMarts = pmnDataMartIDs.Where(id => !userDataMarts.Any(dm => id == dm.DataMartID)).ToArray();
                        if (newDataMarts.Any())
                        {
                            var pmnDataMarts = (await pmnClient.GetDataMartMetadata(newDataMarts)).ToArray();
                            foreach (var newID in newDataMarts)
                            {
                                //confirm the datamart is registered, and the user is associated
                                if ((await modelDb.DataMarts.AnyAsync(dm => dm.ID == newID)) == false)
                                {
                                    var newPMNDataMart = pmnDataMarts.Where(d => d.ID == newID).First();
                                    await modelDb.DataMarts.AddAsync(newPMNDataMart.ToDMCSModel());
                                }

                                await modelDb.UserDataMarts.AddAsync(new UserDataMart { DataMartID = newID, UserID = validatedUser.ID.Value });
                            }
                        }

                        var removedDataMarts = userDataMarts.Where(dm => !pmnDataMartIDs.Contains(dm.DataMartID)).ToArray();
                        if (removedDataMarts.Any())
                        {
                            //remove the user association
                            modelDb.UserDataMarts.RemoveRange(removedDataMarts);
                        }

                        await modelDb.SaveChangesAsync();
                        await trx.CommitAsync();
                    }

                    return Ok();
                }
                catch (HttpRequestException ex)
                {
                    //fallback to local login if an exception was thrown when authenticating against PMN API

                    this.logger.Error(ex, "The authentication request to PopMedNet failed");

                    var result = await _signInManager.PasswordSignInAsync(auth.username, auth.password, false, true);
                    var user = await this.modelDb.Users.Where(x => x.UserName == auth.username).FirstOrDefaultAsync();

                    if (!result.Succeeded)
                    {
                        this.logger.Information("Authentication failed for user: {0}", string.IsNullOrEmpty(auth.username) ? "<unknown>" : auth.username);
                        ModelState.AddModelError("error", "Authentication failed, please reconfirm your username and password.");
                        if (user != null)
                        {
                            await this.modelDb.AuthenticationLogs.AddAsync(new Data.Model.AuthenticationLog { UserID = user.ID, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(), Success = false, Details = Crypto.EncryptStringAES("UserName: " + auth.username + " was attempted with Password:" + auth.password, "AuthenticationLog", user.ID.ToString("D")) });
                            await this.modelDb.SaveChangesAsync();
                        }
                        return BadRequest(ModelState);
                    }

                    if (result.IsLockedOut)
                    {
                        await this.modelDb.AuthenticationLogs.AddAsync(new Data.Model.AuthenticationLog { UserID = user.ID, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(), Success = false, Details = "UserName: " + auth.username + " was locked out" });
                        await this.modelDb.SaveChangesAsync();
                        this.logger.Information("User: {0} is locked out", string.IsNullOrEmpty(auth.username) ? "<unknown>" : auth.username);
                        ModelState.AddModelError("error", "User is Locked out.");
                        return BadRequest(ModelState);
                    }

                    this.logger.Information("Authentication successful for user: {0}", auth.username);
                    await this.modelDb.AuthenticationLogs.AddAsync(new Data.Model.AuthenticationLog { UserID = user.ID, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(), Success = true });
                    await this.modelDb.SaveChangesAsync();

                    var cookie = Crypto.EncryptStringAES(string.Format("{0}:{1}:{2}", auth.username, auth.password, Guid.NewGuid()), _config.Value.Settings.Hash, _config.Value.Settings.Key);
                    var cookieOptions = new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true
                    };
                    Response.Cookies.Append("DMCS-User", cookie, cookieOptions);

                    return Ok();
                }
                catch (ServiceRequestException<UserDTO> ex)
                {
                    this.logger.Error(ex, "The authentication request to PopMedNet failed");
                    foreach (var err in ex.Errors)
                    {
                        ModelState.AddModelError("error", err.Description);
                    }
                    return BadRequest(ModelState);
                }
                catch (Exception ex)
                {
                    var user = await this.modelDb.Users.Where(x => x.UserName == auth.username).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        await this.modelDb.AuthenticationLogs.AddAsync(new Data.Model.AuthenticationLog { UserID = user.ID, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(), Success = false, Details = Crypto.EncryptStringAES("UserName: " + auth.username + " was attempted with Password:" + auth.password, "AuthenticationLog", user.ID.ToString("D")) });
                        await this.modelDb.SaveChangesAsync();
                    }

                    ModelState.AddModelError("error", ex.Message);
                    return BadRequest(ModelState);
                }
            }
        }

        [Authorize]
        [HttpGet("touch")]
        public async Task<IActionResult> Touch([FromServices] IHubContext<SessionHub> sessionHub)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identityUser = await _signInManager.UserManager.FindByNameAsync(User.Identity.Name);
                await _signInManager.RefreshSignInAsync(identityUser);

                string username = User.Identity.Name;
                _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
                {                    
                    await sessionHub.Clients.Groups(username).SendAsync("onSessionRefresh", DateTime.UtcNow);
                });

                return Ok();
            }

            return BadRequest(new { Error = "User is not authenticated." });
        }

        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromServices] IHubContext<SessionHub> sessionHub)
        {
            if (User.Identity.IsAuthenticated)
            {
                this.logger.Information("Signing out for user: {0}", User.Identity.Name);
            }

            string username = User.Identity.Name;

            await _signInManager.SignOutAsync();

            var cookieOptions = new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(-1)
            };

            Response.Cookies.Append("DMCS-User", "", cookieOptions);

            //await sessionHub.Clients.Groups(username).SendAsync("onLogout");

            if (!string.IsNullOrEmpty(username))
            {
                _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await sessionHub.Clients.Groups(username).SendAsync("onLogout");
                });
            }

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(PMNApi.PMNDto.ForgotPasswordDTO data)
        {
            using(var api = new PMNApi.PMNApiClient(_config.Value.PopMedNet))
            {
                await api.ForgotPassword(data);
            }

            return Ok();
        }

        public class AuthenticationRequest
        {
            public string username { get; set; }

            public string password { get; set; }
        }
    }
}
