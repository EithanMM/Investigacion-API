using AutoMapper;
using Investigacion.Core;
using Investigacion.Core.Excepciones;
using Investigacion.DataAccess;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Investigador.DTOModels;
using Investigacion.Model.Rol.DTOModels;
using Investigacion.Model.TipoTrabajo.DTOModels;
using Investigacion.Model.Trabajo.DTOModels;
using Investigacion.Model.Usuario.DTOModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Investigacion.WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            /****************** SWAGGER JSON PROPERTIES ********************/
            services.AddMvcCore()
                .AddJsonOptions(options => { /*Propiedad que omite propiedades del JSON con null. */
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                }).AddApiExplorer();
            /*********** DISABLEDING EXPLICIT MODEL VALIDATION ***************/
            services.AddControllers(options => { /* Uso de clase personalizada para excepciones */
                options.Filters.Add<ExcepcionFiltro>();

            }).ConfigureApiBehaviorOptions(options => { /* Deshabilita el Model.IsValid */
                options.SuppressModelStateInvalidFilter = true;
            });
            /****************************************************************/

            /*********************** AUTOMAPPER ****************************/
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            /****************************************************************/

            /******************** CONNECTION STRING *************************/
            //services.AddDbContext<InvestigacionDBContext>(option =>  // <- Usando DBcontext generado por E.F
            //option.UseSqlServer(Configuration.GetConnectionString("INVESTIGACION_DB")));
            /****************************************************************/

            /*********************** INJECT DEPENDENCY II ******************************/

            /*********************** INTERFACE - CORE ***********************************/
            /**************************** LECTURA ***************************************/
            services.AddTransient<ILecturaCore<InvestigadorModel>, InvestigadorCore>();
            services.AddTransient<ILecturaCore<TipoTrabajoModel>, TipoTrabajoCore>();
            services.AddTransient<ILecturaCore<TrabajoModel>, TrabajoCore>();
            services.AddTransient<ILecturaCore<UsuarioModel>, UsuarioCore>();
            services.AddTransient<ILecturaCore<RolModel>, RolCore>();
            /*************************** ESCRITURA **************************************/
            services.AddTransient<IEscrituraCore<InvestigadorModel, AgregarInvestigadorDTO, ActualizarInvestigadorDTO>, InvestigadorCore>();
            services.AddTransient<IEscrituraCore<TipoTrabajoModel, AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO>, TipoTrabajoCore>();
            services.AddTransient<IEscrituraCore<TrabajoModel, AgregarTrabajoDTO, ActualizarTrabajoDTO>, TrabajoCore>();
            services.AddTransient<IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO>, UsuarioCore>();
            services.AddTransient<IEscrituraCore<RolModel, AgregarRolDTO, ActualizarRolDTO>, RolCore>();
            /************************** ELIMINACION *************************************/
            services.AddTransient<IEliminarCore<InvestigadorModel>, InvestigadorCore>();
            services.AddTransient<IEliminarCore<TipoTrabajoModel>, TipoTrabajoCore>();
            /************************** SEGURIDAD *************************************/
            services.AddTransient<ISeguridadCore<ActualizarPasswordDTO>, UsuarioCore>();


            /*********************** INTERFACE - DATA ACCESS *****************************/
            /**************************** LECTURA ***************************************/
            services.AddTransient<ILecturaDataAccess<InvestigadorModel>, InvestigadorDataAccess>();
            services.AddTransient<ILecturaDataAccess<TipoTrabajoModel>, TipoTrabajoDataAccess>();
            services.AddTransient<ILecturaDataAccess<TrabajoModel>, TrabajoDataAccess>();
            services.AddTransient<ILecturaDataAccess<UsuarioModel>, UsuarioDataAccess>();
            services.AddTransient<ILecturaDataAccess<RolModel>, RolDataAccess>();
            /*************************** ESCRITURA **************************************/
            services.AddTransient<IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO>, InvestigadorDataAccess>();
            services.AddTransient<IEscrituraDataAccess<AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO>, TipoTrabajoDataAccess>();
            services.AddTransient<IEscrituraDataAccess<AgregarTrabajoDTO, ActualizarTrabajoDTO>, TrabajoDataAccess>();
            services.AddTransient<IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO>, UsuarioDataAccess>();
            services.AddTransient<IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO>, RolDataAccess>();
            /************************** ELIMINACION *************************************/
            services.AddTransient<IEliminarDataAccess<InvestigadorModel>, InvestigadorDataAccess>();
            services.AddTransient<IEliminarDataAccess<TipoTrabajoModel>, TipoTrabajoDataAccess>();
            /************************** SEGURIDAD *************************************/
            services.AddTransient<ISeguridadDataAccess<ActualizarPasswordDTO>, UsuarioDataAccess>();
            /**************************************************************************/



            /************************ AUTHENTICATION ************************/
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => { /*Configuracion de validacion JWT*/
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true, /* <= aplicacion cliente */
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true, /* <= valida la firma del emisor*/
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });
            /****************************************************************/

            /************************ SWAGGER API ****************************/
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo {
                        Title = "Investigacion API",
                        Description = "API refinada orientada a microservicios",
                        Version = "v1"
                    });

                string FileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string FilePath = Path.Combine(AppContext.BaseDirectory, FileName);
                options.IncludeXmlComments(FilePath);
            });
            /****************************************************************/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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
