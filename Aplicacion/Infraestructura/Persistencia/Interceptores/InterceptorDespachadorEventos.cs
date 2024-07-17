using Aplicacion.Dominio.Comunes;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.Persistencia.Interceptores
{
    public class InterceptorDespachadorEventos : SaveChangesInterceptor
    {
        private readonly IMediator mediator;

        public InterceptorDespachadorEventos(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            DespacharEventosEnSegundoPlano(eventData.Context);
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
            CancellationToken cancellationToken = new())
        {
            DespacharEventosEnSegundoPlano(eventData.Context);
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
        private void DespacharEventosEnSegundoPlano(DbContext? contexto)
        {
            if (contexto is null) return;
            Task.Run(async () => await DespacharEventos(contexto));
        }
        public async Task DespacharEventos(DbContext? contexto)
        {
            if (contexto is null) return;

            var entidades = contexto.ChangeTracker.Entries<EntidadBase>()
                .Where(e =>
                    e.Entity.ObtenerEventosDominios().Count != 0
                )
                .Select(e => e.Entity);
            var eventos = entidades.SelectMany(e => e.ObtenerEventosDominios()).ToList();
            entidades.ToList().ForEach(e => e.LimpiarEventosDominios());
            var tareas = eventos.Select(async evento => await this.mediator.Publish(evento));
            await Task.WhenAll(tareas);
        }
    }
}
