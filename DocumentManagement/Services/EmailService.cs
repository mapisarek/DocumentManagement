using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon;
using Amazon.Runtime;
using MimeKit;
using System.IO;
using DocumentManagement.DAL;
using System.Reflection;

namespace DocumentManagement.Services
{
    public class EmailService
    {
        private BodyBuilder MessageBody;
        private UserService _userService;
        public string wwwPath;
        public List<string> listOfEmails;

        public EmailService(DMContext _dMContext, string path)
        {
            wwwPath = path;
            listOfEmails = new List<string>();
            _userService = new UserService(_dMContext);
        }


        public void createNewEmail(string name, string desc, bool notify, string emailTo, string filePath)
        {
            if (notify)
            {
                listOfEmails = _userService.GetAllEmails();
                foreach (var item in listOfEmails)
                {
                    SendEmails(notifyMemoryStream(item, "A new file has been added to the database", wwwPath, name, desc, notify, filePath));
                }
            }
            else
                SendEmails(notifyMemoryStream(emailTo, "Special delivery from the DM Team", wwwPath, name, desc, notify, filePath));

        }

        public static BodyBuilder emailBody(string wwwPath, string fileTitle, string description, bool notify, string path)
        {
            string template = null;

            if (notify)
                template = "Notify.html";
            else
                template = "ItemSend.html";
            string fullPath = wwwPath + "/EmailTemplate/" + template;
            string htmlString = File.ReadAllText(fullPath);
            htmlString = htmlString.Replace("FILETITLE", fileTitle);
            htmlString = htmlString.Replace("DESCRIPTIONITEM", description);

            var body = new BodyBuilder()
            {
                HtmlBody = htmlString,
                TextBody = "Automatic notification",
            };
            if (path != null)
                body.Attachments.Add(path);

            return body;
        }



        private static MimeMessage GetMessage(string sendToAddress, BodyBuilder bodyBuilder, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("System ZD", "zarzadzaniedokumentami@gmail.com"));
            message.To.Add(new MailboxAddress(string.Empty, sendToAddress));
            message.Subject = subject;
            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }

        private static MemoryStream notifyMemoryStream(string sendToAddress, string subject, string path, string fileName, string desc, bool automatic, string filePath)
        {
            var stream = new MemoryStream();
            GetMessage(sendToAddress, emailBody(path, fileName, desc, automatic, filePath), subject).WriteTo(stream);
            return stream;
        }

        public void SendEmails(MemoryStream messageStream)
        {
            var credentals = new BasicAWSCredentials("AKIAJKFGC76AKAMIJ7KQ", "7QHGvWfljz8aJ4DWD/LRhq+WxflaShugiEKtZREf");
            using (var client = new AmazonSimpleEmailServiceClient(credentals, RegionEndpoint.EUWest1))
            {
                var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(messageStream) };
                try
                {
                    var response = client.SendRawEmailAsync(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + e.Message);
                }
            }

        }
    }
}
