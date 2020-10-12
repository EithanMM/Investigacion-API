using AutoMapper;
using Investigacion.WebApi.Extensiones;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Investigacion.WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            /******************** SESSION STORAGE **************************/
            services.AddSessionConfiguration(Configuration);
            /****************** SWAGGER JSON PROPERTIES ********************/
            services.AddSwaggerJsonConfiguration(Configuration);

            /*********** DISABLEDING EXPLICIT MODEL VALIDATION ***************/
            services.DisableExplicitModelValidation(Configuration);

            /****************** ENABLING CUSTOM CLASSES **********************/
            services.AddCustomClassConfiguration(Configuration);

            /*********************** AUTOMAPPER ****************************/
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            /*********************** INJECT DEPENDENCY II ******************************/
            services.AddDependencyInjectionCore(Configuration);
            services.AddDependencyInjectionDataAccess(Configuration);

            /************************ AUTHENTICATION ************************/
            services.AddAuthenticationTokenConfiguration(Configuration);

            /************************ PASSWORD CONFIG ***********************/
            services.AddPasswordConfiguration(Configuration);

            /************************ SWAGGER API ****************************/
            services.AddSwaggerConfiguration(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Add User session
            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            // Inicializa el swagger.
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Investigacion API");
                options.DocExpansion(DocExpansion.None);
            });
        }
    }
}
