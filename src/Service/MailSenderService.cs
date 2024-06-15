using HandlebarsDotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TucaAPI.src.Dtos.Mail;
using TucaAPI.src.Interfaces;

namespace TucaAPI.src.Service
{
    public class MailSenderService : IMailSenderService
    {
        private readonly MailSettings mailSettings;

        public MailSenderService(IOptions<MailSettings> mailSettingsOptions)
        {
            this.mailSettings = mailSettingsOptions.Value;
        }

        private async Task<bool> BaseSendMailAsync<T>(T mailData, BodyBuilder emailBodyBuilder) where T : BaseMailData
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(this.mailSettings.SenderName, this.mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.EmailSubject;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(this.mailSettings.Server, this.mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(this.mailSettings.UserName, this.mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendAsync<T>(T mailData) where T : BaseMailData
        {
            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.TextBody = mailData.EmailBody;
            return await this.BaseSendMailAsync<T>(mailData, emailBodyBuilder);
        }

        public async Task<bool> SendHtmlAsync<T>(T mailData) where T : BaseHtmlMailData
        {
            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = mailData.TemplateWriter.ToString();
            emailBodyBuilder.TextBody = mailData.EmailBody;

            return await this.BaseSendMailAsync<T>(mailData, emailBodyBuilder);
        }
    }
}