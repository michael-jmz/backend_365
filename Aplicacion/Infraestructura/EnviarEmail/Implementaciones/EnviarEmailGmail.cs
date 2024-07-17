using Aplicacion.Infraestructura.EnviarEmail.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.EnviarEmail.Implementaciones
{
    public class EnviarEmailGmail : IEnviarEmail
    {
        private readonly SmtpClient smtpClient;
        private readonly string email;

        public EnviarEmailGmail(IConfiguration configuracion)
        {
            this.email = configuracion["CredencialesEmail:USER"]!;
            smtpClient = new SmtpClient
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential { 
                    UserName = this.email, 
                    Password = configuracion["CredencialesEmail:PASSWORD"]!
                }
            };
        }
        public async Task Ejecutar(string destinatario, string asunto, string cuerpo, CancellationToken cancellationToken)
        {
            MailMessage mailMessage = new(this.email, destinatario, asunto, cuerpo);
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
