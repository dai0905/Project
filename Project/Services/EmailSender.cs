
using System.Net;
using System.Net.Mail;

namespace Project.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("thoiloan2025@gmail.com", "nohoprsovbkhncyc")
            };

            return client.SendMailAsync(
                new MailMessage(from: "thoiloan2025@gmail.com",
                                  to: email,
                                  subject,
                                  message
            ));
        }
    }
}
