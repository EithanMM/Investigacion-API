﻿using Investigacion.Core;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Investigacion.WebApi.Extensiones {
    public static class ServicesCollectionExtension {

        /// <summary>
        /// Obtenemos la configuracion para el password que existe en el appsettings.json
        /// </summary>
        public static void AddPasswordConfiguration(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<PasswordConfigModel>(options => configuration.GetSection("PasswordConfig").Bind(options));
        }

        /// <summary>
        /// Agrega injeccion de dependencias para la capa Core
        /// </summary>
        public static void AddDependencyInjectionCore(this IServiceCollection services, IConfiguration configuration) {

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
            services.AddTransient<IEliminarCore, InvestigadorCore>();
            services.AddTransient<IEliminarCore, TipoTrabajoCore>();

            /************************** SEGURIDAD *************************************/
            services.AddTransient<ISeguridadCore<ActualizarPasswordDTO, AccesoUsuarioDTO, RespuestaUsuarioDTO>, UsuarioCore>();
        }

        /// <summary>
        /// Agrega injeccion de dependencias para la capa DataAccess
        /// </summary>
        public static void AddDependencyInjectionDataAccess(this IServiceCollection services, IConfiguration configuration) {

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
            services.AddTransient<IEliminarDataAccess, InvestigadorDataAccess>();
            services.AddTransient<IEliminarDataAccess, TipoTrabajoDataAccess>();

            /************************** SEGURIDAD *************************************/
            services.AddTransient<ISeguridadDataAccess<ActualizarPasswordDTO, AccesoUsuarioDTO>, UsuarioDataAccess>();
        }

        /// <summary>
        /// Configuracion para el uso de JSON en el Swagger
        /// </summary>
        public static void AddSwaggerJsonConfiguration(this IServiceCollection services, IConfiguration configuration) {
            services.AddMvcCore().AddJsonOptions(options => {
                options.JsonSerializerOptions.IgnoreNullValues = true; /*Propiedad que omite propiedades del JSON con null. */
            }).AddApiExplorer();
        }

        /// <summary>
        /// Deshabilita el Model.IsValid que viene por defecto.
        /// </summary>
        public static void DisableExplicitModelValidation(this IServiceCollection services, IConfiguration configuration) {
            services.AddControllers().ConfigureApiBehaviorOptions(options => {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        /// <summary>
        /// Configuracion para uso de clases personalizadas.
        /// </summary>
        public static void AddCustomClassConfiguration(this IServiceCollection services, IConfiguration configuration) {
            services.AddControllers(options => { /* Uso de clase personalizada para excepciones */
                options.Filters.Add<ExcepcionFiltro>();
            });
        }

        /// <summary>
        /// Generamos configuracion para el Swagger
        /// </summary>
        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration Configuration) {
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

                /*********************** SWAGGER AUTH ****************************/
                var SecuritySchema = new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                options.AddSecurityDefinition("Bearer", SecuritySchema);

                var SecurityRequirement = new OpenApiSecurityRequirement();
                SecurityRequirement.Add(SecuritySchema, new[] { "Bearer" });
                options.AddSecurityRequirement(SecurityRequirement);
                /****************************************************************/
            });
        }

        /// <summary>
        /// Configuracion para la autenticacion del JWT
        /// </summary>
        public static void AddAuthenticationTokenConfiguration(this IServiceCollection services, IConfiguration Configuration) {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                /*Configuramos la validacion que se hace desde el JWT*/
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true, /* <= aplicacion cliente */
                    ValidateAudience = true,
                    ValidateLifetime = true, /* <= validamos el tiempo de vida del token*/
                    ClockSkew = TimeSpan.Zero, /*Setea el tiempo que espera el token una vez que expira*/
                    ValidateIssuerSigningKey = true, /* <= valida la firma del emisor*/
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });
        }

        /// <summary>
        /// Obtenemos la configuracion de la BD (Haciendo uso de EF Core)
        /// </summary>
        //public static void AddDataBaseConfiguration(this IServiceCollection services, IConfiguration configuration) {
        //    services.AddDbContext<InvestigacionDBContext>(option =>  // <- Usando DBcontext generado por E.F
        //    option.UseSqlServer(Configuration.GetConnectionString("INVESTIGACION_DB")));
        //}

    }
}
