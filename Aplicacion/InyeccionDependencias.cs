using Aplicacion.Helper.Comportamientos;
using Aplicacion.Infraestructura.EnviarEmail.Implementaciones;
using Aplicacion.Infraestructura.EnviarEmail.Interfaces;
using Aplicacion.Infraestructura.Persistencia;
using Aplicacion.Infraestructura.Persistencia.Interceptores;
using Aplicacion.Infraestructura.RegistroCivil.Implementaciones;
using Aplicacion.Infraestructura.RegistroCivil.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AddAplicacion(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRegistroCivil,RegistroCivilLocal>();
            services.AddScoped<IEnviarEmail, EnviarEmailGmail>();
            services.AddScoped<InterceptorDespachadorEventos>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddLogging();

            services.AddDbContext<ContextoDB>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    (a) => a.MigrationsAssembly("ApiBanca"));
            },
            ServiceLifetime.Transient);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Validacion<,>));


            return services;
        }
    }
}
