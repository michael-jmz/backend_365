using CuentaDominio = Aplicacion.Dominio.Entidades.Cuenta.Cuenta;
using Aplicacion.Infraestructura.EnviarEmail.Interfaces;
using Aplicacion.Infraestructura.Persistencia;
using Aplicacion.Infraestructura.RegistroCivil.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class VerificarCuenta
    {
        public record VerificarCuentaDTO(string Email, string Codigo);
        public record Comando(VerificarCuentaDTO DatosVerificarCuenta):IRequest<RespuestaDTO>;
        public class Validador : AbstractValidator<Comando>
        {
            public Validador()
            {
                RuleFor(x => x.DatosVerificarCuenta.Codigo).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosVerificarCuenta.Email).NotEmpty().EmailAddress().MaximumLength(50);
            }
        }
        public class Handler : IRequestHandler<Comando, RespuestaDTO>
        {
            private readonly ContextoDB contextoDB;
            private readonly IEnviarEmail enviarEmail;
            public Handler(ContextoDB contextoDB, IEnviarEmail enviarEmail)
            {
                this.contextoDB = contextoDB;
                this.enviarEmail = enviarEmail;
            }


            public async Task<RespuestaDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var codigoVerificacion = await contextoDB.CodigoVerificacion
                    .Include(x => x.Usuario)
                    .Where(x => x.Usuario.Email == request.DatosVerificarCuenta.Email.ToLower())
                    .OrderByDescending(x => x.FechaCreacion)
                    .FirstOrDefaultAsync(cancellationToken);
                codigoVerificacion.ThrowIfNull(x => new Exception("El email no existe!"));
                codigoVerificacion.Throw(x => new Exception("El codigo no coincide!"))
                    .IfTrue(x => x.Codigo != request.DatosVerificarCuenta.Codigo);
                codigoVerificacion.Throw(x => new Exception("El codigo ya expiro!"))
                    .IfTrue(x => DateTime.Compare(x.FechaExpiracion,DateTime.Now) < 0);
                var usuario = await contextoDB.Usuario
                    .Where(x => x.Email == request.DatosVerificarCuenta.Email.ToLower())
                    .FirstAsync(cancellationToken);

                var cuenta = new CuentaDominio() {
                    IdUsuario = usuario.Id,
                    Numero = ((await contextoDB.Cuenta.ToArrayAsync()).Length + 220100).ToString(),
                    Saldo = 0
                };

                usuario.CodigoVerificado = true;
                var password = new Random().NextInt64(10000000,99999999).ToString();
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(password);
                //Enviar email con la contraseña y el numero de cuenta
                await this.enviarEmail.Ejecutar(
                    usuario.Email, 
                    "Credenciales de acceso y Cuenta bancaria", 
                    $"""
                        <p>
                            Email: {usuario.Email} <br>
                            Contraseña: {password} <br>
                        </p>
                        <P>
                            Cuenta bancaria: {cuenta.Numero}
                        </p>
                    """,
                    cancellationToken);

                contextoDB.Update(usuario);
                await contextoDB.AddAsync(cuenta);
                await contextoDB.SaveChangesAsync(cancellationToken);
                return new RespuestaDTO(cuenta.Numero);
            }
        }
        public record RespuestaDTO(string NumeroCuenta);
    }
}
