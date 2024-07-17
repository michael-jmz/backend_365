using Aplicacion.Dominio.Entidades.RegistroCivil;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.Persistencia.Configuracion
{
    public class EcuatorianoConfiguracion : IEntityTypeConfiguration<Ecuatoriano>
    {
        public void Configure(EntityTypeBuilder<Ecuatoriano> entity)
        {
            entity.Property(x => x.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(x => x.CodigoDactilar)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
