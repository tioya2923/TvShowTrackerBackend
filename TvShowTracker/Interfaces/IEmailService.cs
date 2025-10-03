using System.Threading.Tasks;

namespace TvShowTracker.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Envia um email simples com assunto e corpo.
        /// </summary>
        /// <param name="toEmail">Email de destino</param>
        /// <param name="subject">Assunto do email</param>
        /// <param name="body">Conteúdo do email (HTML ou texto)</param>
        Task SendAsync(string toEmail, string subject, string body);
    }
}
