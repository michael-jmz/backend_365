using Aplicacion.Dominio.Entidades.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.Persistencia.Configuracion
{
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.Property(x => x.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.CodigoDactilar)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Celular)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);


            entity.Property(x => x.Password)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Provincia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(e => e.FotoRostroURL)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.SituacionLaboral)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Empresa)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(x => x.PaisPagoImpuestos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.FechaCreacion)
                .IsRequired();
        }
    }
}
