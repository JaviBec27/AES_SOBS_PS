using AES_SOBS_PS.Services.SecurityServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.MailServices
{
    public class MessageServiceMail : IMessagingServiceMail
    {
        private readonly IConfiguration configuration;

        public MessageServiceMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(string toName, string toemailAddress, string subject, string message, string[] attachmentsFilePath)
        {
            string fromDisplayName = "AES_SOBS_Sysmtem";
            string fromEmailAddress = configuration["EmailSender"];
            string fromPassword = CriptoSecurity.Decrypt(configuration["KeyMailSender"]);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromDisplayName, fromEmailAddress));
            email.To.Add(new MailboxAddress(toName, toemailAddress));
            email.Subject = subject;

            var body = new BodyBuilder { HtmlBody = message };

            if (attachmentsFilePath != null)
            {
                foreach (var itemPath in attachmentsFilePath)
                {
                    body.Attachments.Add(itemPath);
                }
            }

            email.Body = body.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    //client.ServerCertificateValidationCallback = (sender, certificate, certChainType, errors) => true;
                    client.AuthenticationMechanisms.Remove("XOAUT2");


                    await client.ConnectAsync("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(fromEmailAddress, fromPassword);

                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
