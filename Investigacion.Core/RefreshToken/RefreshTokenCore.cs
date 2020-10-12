using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class RefreshTokenCore : ITokenCore<UsuarioModel, RefreshTokenModel> {

        #region Variables y constructor
        private static int PosicionMensajeError = 6;
        private readonly IConfiguration Configuracion;
        private readonly ITokenDataAccess<RefreshTokenModel> ITokenRefreshToken;

        public RefreshTokenCore(ITokenDataAccess<RefreshTokenModel> ITokenRefreshToken, IConfiguration Configuracion) {
            this.Configuracion = Configuracion;
            this.ITokenRefreshToken = ITokenRefreshToken;
        }
        #endregion

        #region Metodos
        public string GenerarToken(UsuarioModel Modelo) {

            // Generacion de Header
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuracion["Authentication:SecretKey"]));
            var CredencialAcceso = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var Header = new JwtHeader(CredencialAcceso);

            //Claims <- informacion que queremos agregar en el cuerpo del mensaje
            // Para rol, se decidio usar el consecutivo del rol para identificarlo.
            var Claims = new[] {
                    new Claim(ClaimTypes.Name, Modelo.Usuario),
                    new Claim(ClaimTypes.Email, Modelo.Email),
                    new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddMinutes(1).ToString()),
                    new Claim(ClaimTypes.Role, Modelo.RolModel.Descripcion)
                };

            //Payload
            var Payload = new JwtPayload(
                Configuracion["Authentication:Issuer"],
                Configuracion["Authentication:Audience"],
                Claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(1)
            );

            //Signature
            var Token = new JwtSecurityToken(Header, Payload);

            //Serializar y generar token
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public RefreshTokenModel GenerarRefreshToken(UsuarioModel Modelo) {

            RefreshTokenModel Respuesta = new RefreshTokenModel();
            var NumeroAleatorio = new byte[32];

            using (var Generador = new RNGCryptoServiceProvider()) {
                Generador.GetBytes(NumeroAleatorio);
                Respuesta.IdUsuario = Modelo.Id;
                Respuesta.Token = Convert.ToBase64String(NumeroAleatorio);

                return Respuesta;
            }
        }

        public async Task<RefreshTokenModel> AgregarRefreshToken(RefreshTokenModel Modelo) {

            RefreshTokenModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await ITokenRefreshToken.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RefreshTokenModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));

            return Respuesta;
        }

        public async Task<RefreshTokenModel> Obtener(Guid Id) {

            RefreshTokenModel Respuesta;

            string Resultado = await ITokenRefreshToken.Obtener(Id);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RefreshTokenModel>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El usuario con el id especificado no existe");
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<RefreshTokenModel> Obtener(string Usuario, string Email) {

            RefreshTokenModel Respuesta;

            if (Usuario.Equals("")) throw new ExcepcionCore("No se brindo el nombre de usuario");
            if (Email.Equals("")) throw new ExcepcionCore("No se brindo el email");

            string Resultado = await ITokenRefreshToken.Obtener(Usuario, Email);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RefreshTokenModel>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El usuario " + Usuario + " no existe");
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<RefreshTokenModel> GenerarTokenAcceso(ClaimsIdentity Identidad) {

            RefreshTokenModel Respuesta;

            string Nombreusuario = Identidad.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            string Email = Identidad.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            Respuesta = await Obtener(Nombreusuario, Email);

            if (DateTime.UtcNow < Respuesta.FechaExpiracion) {
                Respuesta.Token = GenerarToken(Respuesta.UsuarioModel);
            }
            return Respuesta;
        }

        public string GenerarToken(JwtSecurityToken TokenS) {

            string Usuario = TokenS.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            string Correo = TokenS.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            string Rol = TokenS.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();

            // Generacion de Header
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuracion["Authentication:SecretKey"]));
            var CredencialAcceso = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var Header = new JwtHeader(CredencialAcceso);

            //Claims <- informacion que queremos agregar en el cuerpo del mensaje
            // Para rol, se decidio usar el consecutivo del rol para identificarlo.
            var Claims = new[] {
                    new Claim(ClaimTypes.Name, Usuario),
                    new Claim(ClaimTypes.Email, Correo),
                    new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddMinutes(1).ToString()),
                    new Claim(ClaimTypes.Role, Rol)
                };

            //Payload
            var Payload = new JwtPayload(
                Configuracion["Authentication:Issuer"],
                Configuracion["Authentication:Audience"],
                Claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(1)
            );

            //Signature
            var Token = new JwtSecurityToken(Header, Payload);

            //Serializar y generar token
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public JwtSecurityToken DecodificarToken(string palabra) {

            var Handler = new JwtSecurityTokenHandler();
            var TokenS = Handler.ReadToken(palabra) as JwtSecurityToken;
            return TokenS;
        }

        public string ObtenerFechaToken(JwtSecurityToken Token) {

            string Fecha = Token.Claims.Where(c => c.Type == ClaimTypes.Expiration).Select(c => c.Value).SingleOrDefault();
            return Fecha;
        }

        public bool CompararFechas(DateTime FechaComparacion) {
            return (FechaComparacion < DateTime.UtcNow) ? true : false;
        }
        #endregion
    }
}
