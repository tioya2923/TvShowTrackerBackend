using System.Net;
using System.Net.Mail;
using TvShowTracker.Interfaces;

namespace TvShowTracker.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var simulate = _config.GetValue<bool>("Email:Simulate");

            if (simulate)
            {
                // Modo simulado (ex: ambiente de desenvolvimento)
                Console.WriteLine($"[SIMULADO] Email para: {toEmail}");
                Console.WriteLine($"[SIMULADO] Assunto: {subject}");
                Console.WriteLine($"[SIMULADO] Corpo:\n{body}");
                await Task.CompletedTask;
                return;
            }

            // Modo real (ex: produção)
            var smtpHost = _config["Email:SmtpHost"];
            var smtpPort = int.TryParse(_config["Email:SmtpPort"], out var port) ? port : 587;
            var smtpUser = _config["Email:SmtpUser"];
            var smtpPass = _config["Email:SmtpPass"];
            var fromEmail = _config["Email:From"];

            if (string.IsNullOrWhiteSpace(fromEmail))
                throw new InvalidOperationException("Email:From não está configurado.");

            var message = new MailMessage(fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
