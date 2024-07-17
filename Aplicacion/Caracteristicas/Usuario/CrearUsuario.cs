using Aplicacion.Dominio.Entidades.CodigoVerificacion;
using Aplicacion.Infraestructura.Persistencia;
using Aplicacion.Infraestructura.RegistroCivil.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Throw;
using UsuarioDominio = Aplicacion.Dominio.Entidades.Usuario.Usuario;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class CrearUsuario
    {
        public record CrearUsuarioDTO(
            string Nombres,
            string Apellidos,
            string Cedula,
            string CodigoDactilar,
            string Celular,
            string Email,
            string Provincia,
            string RostroBase64,
            string SituacionLaboral,
            string? Empresa,
            string PaisPagoImpuestos,
            bool AceptoTerminosYConcidiones
        );
        public record Comando(CrearUsuarioDTO DatosCrearUsuario):IRequest<RespuestaDTO>;

        public class Validador : AbstractValidator<Comando>
        {
            public Validador()
            {
                RuleFor(x => x.DatosCrearUsuario.Nombres).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.Apellidos).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.Cedula).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.CodigoDactilar).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.Email).NotEmpty().EmailAddress().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.Provincia).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.RostroBase64).NotEmpty();
                RuleFor(x => x.DatosCrearUsuario.SituacionLaboral).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.Empresa).Must(x => x.IsNullOrEmpty() || x!.Length <= 50);
                RuleFor(x => x.DatosCrearUsuario.PaisPagoImpuestos).NotEmpty().MaximumLength(50);
                RuleFor(x => x.DatosCrearUsuario.AceptoTerminosYConcidiones).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Comando, RespuestaDTO>
        {
            private readonly ContextoDB contextoDB;
            private readonly IRegistroCivil registroCivil;
            private readonly IMapper mapper;

            public Handler(ContextoDB contextoDB, IMapper mapper, IRegistroCivil registroCivil)
            {
                this.contextoDB = contextoDB;
                this.registroCivil = registroCivil;
                this.mapper = mapper;
            }


            public async Task<RespuestaDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var usuario = mapper.Map<UsuarioDominio>(request.DatosCrearUsuario);
                var usuarioTemporal = await contextoDB.Usuario.Where(x => x.Email == request.DatosCrearUsuario.Email.ToLower()).FirstOrDefaultAsync();
                if (usuarioTemporal != null)
                {
                    usuarioTemporal.Throw(x => new Exception("El email ya está registrado")).IfTrue(x => x.CodigoVerificado);
                    this.contextoDB.Update(usuarioTemporal);
                }
                else
                {
                    (await this.registroCivil.ValidarDatosCedula(usuario.Cedula, usuario.CodigoDactilar))
                            .Throw(x => new Exception("La cedula no es válida")).IfFalse();
                    (await this.registroCivil.ValidarRostro(request.DatosCrearUsuario.RostroBase64))
                            .Throw(x => new Exception("El rostro no es válido")).IfFalse();
                    
                    await this.contextoDB.AddAsync(usuario, cancellationToken);
                }
                await this.contextoDB.SaveChangesAsync(cancellationToken);
                var codigoVerificacion = CodigoVerificacion.Crear(usuarioTemporal ?? usuario);
                await contextoDB.AddAsync(codigoVerificacion, cancellationToken);
                await this.contextoDB.SaveChangesAsync(cancellationToken);
                return new RespuestaDTO($"Codigo enviado a {request.DatosCrearUsuario.Email} expira en 5 minutos");
            }

            public class MapRespuesta:Profile
            {
                public MapRespuesta()
                {
                    CreateMap<CrearUsuarioDTO, UsuarioDominio>().ConstructUsing((x,b) => UsuarioDominio.CrearUsuario(
                        x.Nombres, x.Apellidos, x.Cedula, x.CodigoDactilar, x.Email.ToLower(), x.Provincia, x.SituacionLaboral,
                        x.Empresa, x.PaisPagoImpuestos, x.AceptoTerminosYConcidiones
                    ));
                }
            }
        }
        public record RespuestaDTO(string Mensaje);
    }
}
