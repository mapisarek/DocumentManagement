using RazorEngine.Templating;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Models
{
    public class EmailClient
    {
        private string Login { get; set; }
        private string Password { get; set; }
        private string EmailAddress { get; set; }
        private string Host { get; set; }
        private int Port { get; set; }
        public int Counter { get; set; }

        public EmailClient()
        {
            Login = "AKIA244YY6KY7FGKMUVG";
            Password = "BEOYbK84gZD+QMEPvVzhIPuqWExdI5Y+R+TTiYhmcw/+";
            Host = "email-smtp.eu-west-1.amazonaws.com";
            Port = 25;
            EmailAddress = "zarzadzaniedokumentami@gmail.com";
            Counter = 0;
        }

        public void SendEMail(Email email, List<string> mailTo)
        {
            foreach (string address in mailTo)
            {
                using (var client = new System.Net.Mail.SmtpClient(Host, Port))
                {
                    client.Credentials = new System.Net.NetworkCredential(Login, Password);
                    client.EnableSsl = true;
                    client.Send
                    (
                              EmailAddress,  // Replace with the sender address.
                              address,    // Replace with the recipient address.
                              email.Subject,
                                email.Content
                                );

                    Counter++;
                }
            }
        }
    }
}
