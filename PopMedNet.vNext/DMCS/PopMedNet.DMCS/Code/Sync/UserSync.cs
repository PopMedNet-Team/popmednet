using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using PopMedNet.DMCS.PMNApi;

namespace PopMedNet.DMCS.Code
{
    public class UserSync
    {
        readonly ModelContext db;
        readonly SignInManager<Data.Identity.IdentityUser> signInManager;
        readonly IOptions<DMCSConfiguration> config;
        readonly ILogger logger;
        public UserSync(ModelContext modelDb, SignInManager<Data.Identity.IdentityUser> signInManager, ILogger logger, IOptions<DMCSConfiguration> config)
        {
            this.db = modelDb;
            this.signInManager = signInManager;
            this.config = config;
            this.logger = logger.ForContext<UserSync>();
        }


        public async Task Execute()
        {
            var users = await db.Users.ToArrayAsync();

            if (users.Length == 0)
                return;

            logger.Information("Beginning sync of users details");
            using (var api = new PMNApiClient(config.Value.PopMedNet))
            {
                BaseResponse<PMNApi.PMNDto.UserDetailsDTO> details;
                try
                {
                    details = await api.GetUserDetails(users.Select(u => u.ID).ToArray());
                }catch(Exception ex)
                {
                    logger.Error(ex, "Error getting user details from the PMN API.");
                    return;
                }

                if(details.errors != null && details.errors.Any())
                {
                    logger.Error($"There was an error retrieving user details from the PMN API: { Environment.NewLine }{ string.Join(Environment.NewLine, details.errors.Select(err => "-" + err.Description))}");
                    return;
                }

                if(details.results == null || details.results.Length == 0)
                {
                    logger.Warning("No users were found sync based on the user IDs specified. {0} user IDs were specified.", users.Length);
                    return;
                }

                logger.Information(details.results.Length + " users found to sync details for");

                foreach(var pmnData in details.results)
                {
                    var user = users.FirstOrDefault(u => u.ID == pmnData.ID);
                    if (user == null)
                        throw new Exception("Somehow the user is not found! There should not be a user returned that does not atch the id's specified.");

                    

                    bool updated = false;
                    //update the user if the metadata has changed
                    if((user.UserName + user.Email) != (pmnData.UserName + pmnData.Email))
                    {
                        user.UserName = pmnData.UserName;
                        user.Email = pmnData.Email;
                        await db.SaveChangesAsync();
                        updated = true;
                    }

                    //find the login
                    var login = await signInManager.UserManager.FindByIdAsync(user.ID.ToString());
                    if(login != null)
                    {
                        //only update existing logins, do not create new ones

                        if(!pmnData.Active || pmnData.Deleted)
                        {
                            logger.Information("User: {UserName} ({ID}) is marked as inactive or deleted. Removing login", user.UserName, user.ID);

                            await signInManager.UserManager.DeleteAsync(login);
                            continue;
                        }

                        //update
                        if((login.UserName + login.Email) != (pmnData.UserName + pmnData.Email) || login.PasswordHash != pmnData.PasswordHash)
                        {
                            await signInManager.UserManager.SetUserNameAsync(login, pmnData.UserName);
                            await signInManager.UserManager.SetEmailAsync(login, pmnData.Email);

                            //updating the password hash will automatically call the update user method on the UserManager.
                            await ((Data.Identity.DMCSUserManager)signInManager.UserManager).UpdatePasswordHash(login, pmnData.PasswordHash);
                            
                            updated = true;
                        }
                    }

                    if (updated)
                    {
                        logger.Debug("Updating details for user: {UserName} ({ID})", user.UserName, user.ID);
                    }
                }

                logger.Debug("Finished user details sync");

            }



        }
    }
}
