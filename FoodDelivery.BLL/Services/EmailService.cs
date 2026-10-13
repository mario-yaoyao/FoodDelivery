using FoodDelivery.BLL.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace FoodDelivery.BLL.Services
{
    public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
    {
        private readonly string _smtpHost = configuration["SmtpSettings:SmtpHost"] ?? "";
        private readonly int _smtpPort =
            int.TryParse(configuration["SmtpSettings:SmtpPort"], out var port)
                ? port
                : 0;
        private readonly string _smtpUser = configuration["SmtpSettings:SmtpUser"] ?? ""; 
        private readonly string _smtpPassword = configuration["SmtpSettings:SmtpPassword"] ?? "";

        public async Task<bool> SendEmailAsync(string toName, string toEmail, string subject, string textContent)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("BudgetWise", _smtpUser));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;

                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <div style='font-family:Arial,sans-serif; max-width:600px; margin:0 auto; color:#333;'>
                            <div style='background:#279af1; padding:24px 32px; border-radius:8px 8px 0 0;'>
                                <h1 style='color:white; margin:0; font-size:24px;'>BudgetWise</h1>
                            </div>
                            <div style='background:#ffffff; padding:32px; border:1px solid #e5e7eb; border-top:none; border-radius:0 0 8px 8px;'>
                                <h2 style='color:#279af1; margin-top:0;'>{subject}</h2>
                                <p>Hello <strong>{toName}</strong>,</p>
                                <p>{textContent}</p>
                                <hr style='margin-top:32px; border:none; border-top:1px solid #e5e7eb;' />
                                <p style='font-size:12px; color:#9ca3af; margin-bottom:0;'>© BudgetWise. This is an automated message, please do not reply.</p>
                            </div>
                        </div>
                    "
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Something went wrong while sending email: {Message}", ex.Message);
                return false;
            }
        }
    }
}
