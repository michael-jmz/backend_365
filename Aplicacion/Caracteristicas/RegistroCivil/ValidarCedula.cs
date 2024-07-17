using Aplicacion.Infraestructura.Persistencia;
using Aplicacion.Infraestructura.RegistroCivil.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.RegistroCivil
{
    public class ValidarCedula
    {
        public record DatosValidarCedula(string Cedula, string CodigoDactilar);
        public record Comando(DatosValidarCedula DatosValidarCedula) : IRequest<RespuestaDTO>;
        public class Validador : AbstractValidator<Comando>
        {
            public Validador()
            {
                RuleFor(x => x.DatosValidarCedula.Cedula)
                    .NotEmpty();

                RuleFor(x => x.DatosValidarCedula.CodigoDactilar)
                    .NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Comando, RespuestaDTO>
        {
            private readonly IRegistroCivil registroCivil;

            public Handler(IRegistroCivil registroCivil)
            {
                this.registroCivil = registroCivil;
            }

            public async Task<RespuestaDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var cedulaValida = await registroCivil.ValidarDatosCedula(request.DatosValidarCedula.Cedula, request.DatosValidarCedula.CodigoDactilar);
                cedulaValida.Throw(x => new Exception("Los datos de la cédula no son válidos!")).IfFalse();
                return new RespuestaDTO("Cédula válida");
            }
        }
        public record RespuestaDTO(string Mensaje);
    }
}
