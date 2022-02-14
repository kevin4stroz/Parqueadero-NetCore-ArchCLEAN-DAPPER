using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Events;
using Parqueadero.BussinesLogic.ParqueaderoBI;
using Parqueadero.Core.Interfaces;
using Parqueadero.Infraestructure.Repositories;
using System.Data;
using System.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Parqueadero.WebApi.ExceptionMiddleware;

namespace Parqueadero.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // base de datos
            services.AddTransient<IDbConnection>(
                    (sp) => new SqlConnection(
                        Configuration.GetConnectionString("DbParqueadero")
                 ));

            // logging serilog
            services.AddLogging(logBuilder => {
                // crear logger
                var loggerConfiguration = new LoggerConfiguration()
                                                .MinimumLevel.Information()
                                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                                .MinimumLevel.Override("Microsoft.Hosting.LifeTime", LogEventLevel.Information)
                                                .WriteTo.Console();
                // injectar servicio
                var logger = loggerConfiguration.CreateLogger();
                logBuilder.Services.AddSingleton<ILoggerFactory>(
                    provider => new SerilogLoggerFactory(logger, dispose: false)
                ) ;
            });

            // injeccion de dependencias capa bussineslogic
            services.AddTransient<IParqueaderoBI, ParqueaderoBI>();

            // injeccion de dependencias capa repositorios
            services.AddTransient<ITipoVehiculoRepository, TipoVehiculoRepository>();
            services.AddTransient<IEstablecimientoRepository, EstablecimientoRepository>();
            services.AddTransient<IAparcamientoRepository, AparcamientoRepository>();
            services.AddTransient<IVehiculoRepository, VehiculoRepository>();
            services.AddTransient<IVehiculoAparcamientoRepository, VehiculoAparcamientoRepository>();

            // injeccion de dependencias para unidad de trabajo
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // documentacion de api
            AddSwagger(services);

        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Parqueadero {groupName}",
                    Version = groupName,
                    Description = "Parqueadero API",
                    Contact = new OpenApiContact
                    {
                        Name = "Parqueadero",

                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parqueadero API V1");
            });

            app.UseMiddleware<ExceptionMiddlewareCore>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
