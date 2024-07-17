using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.EnviarEmail.Interfaces
{
    public interface IEnviarEmail
    {
        Task Ejecutar(string destinatario, string asunto, string cuerpo, CancellationToken cancellationToken);
    }
}
