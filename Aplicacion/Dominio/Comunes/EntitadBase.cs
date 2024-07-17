using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Comunes
{
    public abstract class EntidadBase:IEventoDominio
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; private set; } = DateTime.Now;

        private readonly List<IEventoDominio> eventoDominios = new();
        protected EntidadBase()
        {
            
        }

        public IReadOnlyCollection<IEventoDominio> ObtenerEventosDominios() => this.eventoDominios.AsReadOnly();

        public void LimpiarEventosDominios() => this.eventoDominios.Clear();

        protected void AgregarEventoDominio(IEventoDominio eventoDominio) => this.eventoDominios.Add(eventoDominio);
    }
}
