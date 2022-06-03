using Lpp.Dns.DTO;
using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class NewUserRegistrationDialog
    {
        private readonly IPage _page;
        private readonly IFrame _frame;
        private readonly string frameUrl = ConfigurationManager.AppSettings["baseUrl"] + "home/userregistration";



        public NewUserRegistrationDialog(IPage page)
        {
            _page = page;
            _frame = _page.FrameByUrl(frameUrl);
        }

        #region FillControls

        public async Task FillTextInField(UserRegistrationFillables fieldName, string text)
        {
            var locator = "";
            switch(fieldName)
            {
                case UserRegistrationFillables.FirstName:
                    locator = "#txtFirstName";
                    break;
                case UserRegistrationFillables.MiddleName:
                    locator = "#txtmiddleName";
                    break;
                case UserRegistrationFillables.LastName:
                    locator = "#txtLastName";
                    break;
                case UserRegistrationFillables.Title:
                    locator = "#txtTitle";
                    break;
                case UserRegistrationFillables.Email:
                    locator = "#txtEmail";
                    break;
                case UserRegistrationFillables.Phone:
                    locator = "#txtPhone";
                    break;
                case UserRegistrationFillables.Fax:
                    locator = "#txtFax";
                    break;
                case UserRegistrationFillables.OrganizationRequested:
                    locator = "#txtOrganization";
                    break;
                case UserRegistrationFillables.RoleRequested:
                    locator = "#txtRole";
                    break;
                case UserRegistrationFillables.UserName:
                    locator = "#txtUserName";
                    break;
                case UserRegistrationFillables.Password:
                    locator = "#txtPassword";
                    break;
                case UserRegistrationFillables.PasswordConfirm:
                    locator = "#txtConfirmPassword";
                    break;
                default:
                    throw new NotImplementedException($"No locator implemented for field {fieldName}.");
            }
            Console.WriteLine($"Attempting to enter '{text}' into {fieldName}");
            try
            {
                await _frame.FillAsync($"{locator}", text);
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("\tCould not enter text. Stopping test.");
                throw;
            }
        }

        /// <summary>
        /// Fills in the User Registration form using a UserRegistrationDTO object. Fills the ConfirmPassword
        /// text box with the password property of the DTO.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FillInForm(UserRegistrationDTO dto)
        {
            await FillInForm(dto, dto.Password);
        }

        /// <summary>
        /// Fills in the User Registration form using a UserRegistrationDTO object plus a separate string
        /// to go into the ConfirmPassword text box. If not testing using a different password for ConfirmPassword
        /// than Password, consider using the other overload that takes only a DTO.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="passwordConfirm"></param>
        /// <returns></returns>
        public async Task FillInForm(UserRegistrationDTO dto, string passwordConfirm)
        {
            await FillTextInField(UserRegistrationFillables.FirstName, dto.FirstName);
            await FillTextInField(UserRegistrationFillables.LastName, dto.LastName);
            await FillTextInField(UserRegistrationFillables.Title, dto.Title);
            await FillTextInField(UserRegistrationFillables.Email, dto.Email);
            await FillTextInField(UserRegistrationFillables.Phone, dto.Phone);
            await FillTextInField(UserRegistrationFillables.Fax, dto.Fax);
            await FillTextInField(UserRegistrationFillables.OrganizationRequested, dto.OrganizationRequested);
            await FillTextInField(UserRegistrationFillables.RoleRequested, dto.RoleRequested);
            await FillTextInField(UserRegistrationFillables.UserName, dto.UserName);
            await FillTextInField(UserRegistrationFillables.Password, dto.Password);
            await FillTextInField(UserRegistrationFillables.PasswordConfirm, passwordConfirm);
        }
                
        #endregion

        /// <summary>
        /// Submits the User Registration form. Optionally handles alerts following submission, if they are
        /// not needed for the test at hand.
        /// </summary>
        /// <param name="handleAlert">Pass 'true' if no validation of the 'Success' alert is needed.</param>
        /// <returns></returns>
        public async Task Submit(bool handleAlert=false)
        {
            Console.WriteLine("Attempting to click 'Submit' button...");
            try
            {
                await _frame.ClickAsync("#btnSubmit");
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("\tCould not click 'Submit. Stopping test.");
                throw;
            }
            
            if(handleAlert)
            {
                Console.WriteLine("Alert was expected. Attempting to click 'OK' in alert...");
                try
                {
                    await _frame.ClickAsync("#btnOK:visible");
                    Console.WriteLine("\tSuccess!");
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("\tCould not click 'OK'. Stopping test.");
                    throw;
                }
            }
        }

        public async Task Close()
        {
            await _page.ClickAsync(".k-i-close");
        }

        public async Task<LoginPage> SubmissionSuccessful()
        {
            Console.WriteLine("Checking for Success message...");
            var success = await _frame.QuerySelectorAllAsync("text=Request Submitted");

            if (success != null)
            {
                Console.WriteLine("\tSuccess!");
                await _frame.ClickAsync("#btnOK:visible");
                return new LoginPage(_page);
            }
            else
                throw new Exception("Could not verify Success message. Stopping test.");
        }

        #region ValidationChecks
        public async Task<bool> PasswordMatchErrorDisplays()
        {
            bool result = false;
            var errorDisplays = await _frame.QuerySelectorAllAsync("text=Validation Error");
            if(errorDisplays != null)
            {
                Console.WriteLine("Validation error displayed...");
                errorDisplays = await _frame.QuerySelectorAllAsync("text=Passwords do not match.");
                if(errorDisplays !=null)
                {
                    result = true;
                    await _frame.ClickAsync("#btnOK:visible");
                }
                else
                    Console.WriteLine("Validation error message was not as expected");
            }
            return result;
        }

        public async Task<bool> DuplicateUserNameErrorDisplays()
        {
            bool result = false;
            var error = await _frame.QuerySelectorAllAsync("text=Registration Error");
            if (error != null)
            {
                Console.WriteLine("Registration error displayed...");
                var message = await _frame.QuerySelectorAllAsync("text=We're sorry but that UserName already Exists");
                if (message != null)
                {
                    result = true;
                    await _frame.ClickAsync("#btnOK:visible");
                }
                else
                    Console.WriteLine("Registration error message was not as expected");
            }
            return result;
        }

        public async Task<bool> ValidationMessageDisplays(string labelName)
        {
            bool result = false;
            var msgText = $"text={labelName} is required";
            var message = await _frame.QuerySelectorAllAsync(msgText);
            if (message != null)
            {
                if (await message[0].IsVisibleAsync())
                    result = true;
                else Console.WriteLine($"Validation message for {labelName} was not visible");
            }
            else
                Console.WriteLine($"Validation message for {labelName} was not as expected");
            return result;
        }
        #endregion

        


    }
}
