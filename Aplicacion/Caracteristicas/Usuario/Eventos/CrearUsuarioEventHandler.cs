using Aplicacion.Dominio.Entidades.CodigoVerificacion;
using Aplicacion.Dominio.Entidades.CodigoVerificacion.Eventos;
using Aplicacion.Infraestructura.EnviarEmail.Interfaces;
using Aplicacion.Infraestructura.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Caracteristicas.Usuario.Eventos
{
    public class CrearUsuarioEventHandler : INotificationHandler<CodigoVerificacionCreadoEvent>
    {
        private readonly ContextoDB contextoDB;
        private readonly IEnviarEmail enviarEmail;

        public CrearUsuarioEventHandler(ContextoDB contextoDB, IEnviarEmail enviarEmail)
        {
            this.contextoDB = contextoDB;
            this.enviarEmail = enviarEmail;
        }
        public async Task Handle(CodigoVerificacionCreadoEvent notification, CancellationToken cancellationToken)
        {
            await this.enviarEmail.Ejecutar(
                notification.CodigoVerificacion.Usuario.Email, "CODIGO DE VERIFICACIÓN", 
                $"""
                <h3>Código: {notification.CodigoVerificacion.Codigo}</h3>
                <p>El código es válido hasta <strong>{notification.CodigoVerificacion.FechaExpiracion}</strong></p>
                """,
                cancellationToken);
        }
    }
}
