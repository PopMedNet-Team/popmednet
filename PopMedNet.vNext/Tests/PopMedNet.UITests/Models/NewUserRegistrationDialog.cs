using Lpp.Dns.DTO;
using Microsoft.Playwright;
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
        public async Task EnterFirstName(string firstName)
        {
            await _frame.FillAsync("#txtFirstName", firstName);
        }

        public async Task EnterMiddleName(string middleName)
        {
            await _frame.FillAsync("#txtmiddleName", middleName);
        }

        public async Task EnterLastName(string lastName)
        {
            await _frame.FillAsync("#txtLastName", lastName);
        }

        public async Task EnterTitle(string title)
        {
            await _frame.FillAsync("#txtTitle", title);
        }

        public async Task EnterEmail(string email)
        {
            await _frame.FillAsync("#txtEmail", email);
        }

        public async Task EnterPhone(string phone)
        {
            await _frame.FillAsync("#txtPhone", phone);
        }

        public async Task EnterFax(string fax)
        {
            await _frame.FillAsync("#txtFax", fax);
        }

        public async Task EnterOrganization(string org)
        {
            await _frame.FillAsync("#txtOrganization", org);
        }

        public async Task EnterRole(string role)
        {
            await _frame.FillAsync("#txtRole", role);
        }

        public async Task EnterUserName(string userName)
        {
            await _frame.FillAsync("#txtUserName", userName);
        }

        public async Task EnterPassword(string password)
        {
            await _frame.FillAsync("#txtPassword", password);
        }

        public async Task EnterConfirmPassword(string password)
        {
            await _frame.FillAsync("#txtConfirmPassword", password);
        }

        public async Task Cancel()
        {
            await _frame.ClickAsync("#btnCancel");
        }
        #endregion

        public async Task Submit()
        {
            await _frame.ClickAsync("#btnSubmit");
            await _frame.ClickAsync("#btnOK");
        }

        public async Task Close()
        {
            await _page.ClickAsync(".k-i-close");
        }

        public async Task<bool> SubmissionSuccessful()
        {
            bool result = false;
            var success = await _frame.QuerySelectorAllAsync("text=Request Submitted");
            if(success != null)
            {
                result = true;
                await _frame.ClickAsync("#btnOK");
            }
            return (result);
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
                    await _frame.ClickAsync("#btnOK");
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
                    await _frame.ClickAsync("#btnOK");
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
            await EnterFirstName(dto.FirstName);
            await EnterLastName(dto.LastName);
            await EnterTitle(dto.Title);
            await EnterEmail(dto.Email);
            await EnterPhone(dto.Phone);
            await EnterFax(dto.Fax);
            await EnterOrganization(dto.OrganizationRequested);
            await EnterRole(dto.RoleRequested);
            await EnterUserName(dto.UserName);
            await EnterPassword(dto.Password);
            await EnterConfirmPassword(passwordConfirm);
        }


    }
}
