using UsuarioDominio = Aplicacion.Dominio.Entidades.Usuario.Usuario;
using Aplicacion.Infraestructura.Persistencia;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Autenticacion
{
    public class IniciarSesion
    {
        public record IniciarSesionDTO(string Email, string Password);
        public record Comando(IniciarSesionDTO DatosInicioSesion):IRequest<RespuestaDTO>;
        public class Handler : IRequestHandler<Comando, RespuestaDTO>
        {
            private readonly ContextoDB contextoDB;
            private readonly IMapper mapper;

            public Handler(ContextoDB contextoDB, IMapper mapper)
            {
                this.contextoDB = contextoDB;
                this.mapper = mapper;
            }

            public async Task<RespuestaDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var usuario = await contextoDB.Usuario.Where(x => x.Email == request.DatosInicioSesion.Email.ToLower()).FirstOrDefaultAsync();
                usuario.ThrowIfNull(x => new Exception("Credenciales incorrectas!"));
                usuario.Throw(x => new Exception("Credenciales incorrectas"))
                    .IfFalse(x => BCrypt.Net.BCrypt.Verify(request.DatosInicioSesion.Password, x.Password));
                return mapper.Map<RespuestaDTO>(usuario);
            }

            public class MapRespuesta : Profile
            {
                public MapRespuesta()
                {
                    CreateMap<UsuarioDominio, RespuestaDTO>();
                }
            }
        }
        public record RespuestaDTO(
            string Id,
            string Nombres,
            string Apellidos,
            string Email
        );
    }
}
