using HmsBackend.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace HmsBackend.Services
{
    public class EmailService(IConfiguration config) : IEmailService
    {
        private readonly IConfiguration _config = config;

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.example.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@example.com", "your-password"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage("your-email@example.com", toEmail, subject, message)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
