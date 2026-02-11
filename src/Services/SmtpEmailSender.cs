using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FundNavTracker.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string htmlBody)
        {
            var host = GetSetting("SMTP_HOST", "SMTP:Host");
            var portText = GetSetting("SMTP_PORT", "SMTP:Port");
            var user = GetSetting("SMTP_USER", "SMTP:User");
            var pass = GetSetting("SMTP_PASS", "SMTP:Pass");
            var from = GetSetting("SMTP_FROM", "SMTP:From");

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from))
            {
                _logger.LogWarning("SMTP is not configured. Set SMTP_HOST and SMTP_FROM.");
                return false;
            }

            var port = 587;
            if (!string.IsNullOrWhiteSpace(portText) && int.TryParse(portText, out var parsedPort))
            {
                port = parsedPort;
            }

            using var message = new MailMessage(from, toEmail)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true
            };

            if (!string.IsNullOrWhiteSpace(user))
            {
                client.Credentials = new NetworkCredential(user, pass);
            }

            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP send failed.");
                return false;
            }
        }

        private string? GetSetting(string envKey, string configKey)
        {
            return _configuration[envKey] ?? _configuration[configKey];
        }
    }
}
