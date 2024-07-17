using Aplicacion.Dominio.Entidades.Cuenta;
using Aplicacion.Infraestructura.Persistencia;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class ObtenerCuentasPorUsuario
    {
        public record Consulta(int IdUsuario):IRequest<IReadOnlyCollection<RespuestaDTO>>;
        public class Handler : IRequestHandler<Consulta, IReadOnlyCollection<RespuestaDTO>>
        {
            private readonly ContextoDB contextoDB;
            private readonly IMapper mapper;

            public Handler(ContextoDB contextoDB, IMapper mapper)
            {
                this.contextoDB = contextoDB;
                this.mapper = mapper;
            }
            public async Task<IReadOnlyCollection<RespuestaDTO>> Handle(Consulta request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<RespuestaDTO> cuentas = await contextoDB.Cuenta.Where(x => x.IdUsuario == request.IdUsuario)
                    .ProjectTo<RespuestaDTO>(mapper.ConfigurationProvider)
                    .ToArrayAsync();
                return cuentas;
            }

            public class MapRespuesta : Profile
            {
                public MapRespuesta()
                {
                    CreateMap<Cuenta, RespuestaDTO>();
                }
            }
        }
        public record RespuestaDTO(int Id, string Numero, double Saldo);
    }
}
