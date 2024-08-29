using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.WebSites.Models
{
    public class EmailSettings
    {
        public string From { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public string? Username { get; set; }
        public string? Password { get; set; }

        public static EmailSettings Read(IConfiguration config)
        {
            var settings = new EmailSettings();
            config.GetSection("emailSettings").Bind(settings);
            return settings;
        }

        public System.Net.Mail.SmtpClient CreateSmtpClient()
        {
            var smtpClient = new System.Net.Mail.SmtpClient(Host, Port);
            if (!string.IsNullOrEmpty(Username))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
            }
            return smtpClient;
        }
    }
}
