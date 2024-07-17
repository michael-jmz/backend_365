using Aplicacion.Dominio.Entidades.CodigoVerificacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.Persistencia.Configuracion
{
    public class CodigoVerificacionConfiguracion : IEntityTypeConfiguration<CodigoVerificacion>
    {
        public void Configure(EntityTypeBuilder<CodigoVerificacion> entity)
        {
            entity.Property(x => x.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.FechaExpiracion)
                .IsRequired();

            entity.Property(x => x.FechaCreacion)
                .IsRequired();

            entity.Property(x => x.IdUsuario)
            .IsRequired();

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.CodigosDeVerificacion)
                .HasForeignKey(x => x.IdUsuario);
        }
    }
}
