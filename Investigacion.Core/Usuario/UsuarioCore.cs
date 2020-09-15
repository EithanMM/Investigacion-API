using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Usuario.DTOModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class UsuarioCore : ILecturaCore<UsuarioModel>,
                               IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO>,
                               ISeguridadCore<ActualizarPasswordDTO, AccesoUsuarioDTO, RespuestaUsuarioDTO> {

        #region Variables y constructor
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;
        private readonly IConfiguration Configuracion;
        private readonly PasswordConfigModel PasswordConfiguration;
        private readonly ILecturaDataAccess<UsuarioModel> ILecturaUsuario;
        private readonly ISeguridadDataAccess<ActualizarPasswordDTO, AccesoUsuarioDTO> ISeguridadUsuario;
        private readonly IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO> IEscrituraUsuario;

        public UsuarioCore(ILecturaDataAccess<UsuarioModel> UsuarioLectura, ISeguridadDataAccess<ActualizarPasswordDTO, AccesoUsuarioDTO> UsuarioSeguridad, IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO> UsuarioEscritura, IConfiguration Configuracion, IOptions<PasswordConfigModel> PasswordConfiguration) {
            this.Configuracion = Configuracion;
            this.ILecturaUsuario = UsuarioLectura;
            this.ISeguridadUsuario = UsuarioSeguridad;
            this.IEscrituraUsuario = UsuarioEscritura;
            this.PasswordConfiguration = PasswordConfiguration.Value;
        }
        #endregion

        #region Metodos
        public async Task<UsuarioModel> Agregar(AgregarUsuarioDTO Modelo) {

            UsuarioModel Respuesta;
            AccesoUsuarioDTO Usuario;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");

            Usuario = new AccesoUsuarioDTO(Modelo.Usuario);
            string ExisteUsuario = await ISeguridadUsuario.AutenticarUsuario(Usuario);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<UsuarioModel>(ExisteUsuario);

            if (Respuesta == null) {
                Modelo.Password = HashPassword(Modelo.Password);
                string Resultado = await IEscrituraUsuario.Agregar(Modelo);
                Respuesta = Utf8Json.JsonSerializer.Deserialize<UsuarioModel>(Resultado);
                if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            }
            else throw new ExcepcionCore("Ya existe un usuario con las credenciales brindadas.");

            return Respuesta;
        }

        public Task<UsuarioModel> Actualizar(ActualizarUsuarioDTO Modelo) {
            throw new NotImplementedException();
        }

        public async Task<UsuarioModel> Obtener(string Consecutivo) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioModel>> Listar() {

            var Respuesta = await ILecturaUsuario.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<UsuarioModel> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<UsuarioModel>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<UsuarioModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = (NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = (TamanoPagina > 0) ? TamanoPagina : RegistrosDefault;
            NumeroPagina = (NumeroPagina - 1);

            var Respuesta = await ILecturaUsuario.ListarPaginacion((int)NumeroPagina * (int)TamanoPagina, (int)TamanoPagina);
            EntidadPaginacion<UsuarioModel> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<UsuarioModel>>(Respuesta);
            var RespuestaPaginada = Paginacion<UsuarioModel>.PaginarSQL(Objeto.Data, (int)NumeroPagina, (int)TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }

        public async Task<bool> CambiarPassword(ActualizarPasswordDTO Modelo) {
            throw new NotImplementedException();
        }

        public async Task<RespuestaUsuarioDTO> AutenticarUsuario(AccesoUsuarioDTO Modelo) {

            UsuarioModel Usuario;
            RespuestaUsuarioDTO Resultado;

            string Respuesta = await ISeguridadUsuario.AutenticarUsuario(Modelo);
            Usuario = Utf8Json.JsonSerializer.Deserialize<UsuarioModel>(Respuesta);
            if (Usuario == null) throw new NotFoundExcepcionCore("Las credenciales brindadas no corresponden, verifique el usaurio o el password.");

            if (VerificarPassword(Usuario.Password, Modelo.Password)) {
                Resultado = new RespuestaUsuarioDTO("Bienvenido " + Usuario.Usuario);
                Resultado.Token = GenerarToken(Usuario);
                return Resultado;
            }
            else throw new ExcepcionCore("El password brindado es incorrecto");
        }

        #endregion

        #region Metodos privados
        /// <summary>
        /// Genera un token con la informacion del Usuario.
        /// </summary>
        private string GenerarToken(UsuarioModel Usuario) {
            // Generacion de Header
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuracion["Authentication:SecretKey"]));
            var CredencialAcceso = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var Header = new JwtHeader(CredencialAcceso);

            //Claims <- informacion que queremos agregar en el cuerpo del mensaje
            // Para rol, se decidio usar el consecutivo del rol para identificarlo.
            var Claims = new[] {
                new Claim(ClaimTypes.Name, Usuario.Usuario),
                new Claim(ClaimTypes.Email, Usuario.Email),
                new Claim(ClaimTypes.Role, Usuario.RolModel.Descripcion)
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

        /// <summary>
        /// Meotod para cifrar el password del usuario.
        /// </summary>
        private string HashPassword(string Password) {

            using (var Algoritmo = new Rfc2898DeriveBytes(Password, PasswordConfiguration.SaltSize, PasswordConfiguration.Iteraitons)) {
                var Llave = Convert.ToBase64String(Algoritmo.GetBytes(PasswordConfiguration.KeySize));
                var Salt = Convert.ToBase64String(Algoritmo.Salt);

                return $"{PasswordConfiguration.Iteraitons}.{Salt}.{Llave}";
            }
        }

        /// <summary>
        /// Metodo para verificar que el password brindado corresponde al usuario.
        /// </summary>
        private bool VerificarPassword(string Hash, string Password) {

            var Partes = Hash.Split('.');
            if (Partes.Length != 3) throw new FormatException("Formato de password no esperado.");

            //Dividimos el Hash en 3 partes diferentes
            var Iteraciones = Convert.ToInt32(Partes[0]);
            var Salt = Convert.FromBase64String(Partes[1]);
            var Llave = Convert.FromBase64String(Partes[2]);

            using (var Algoritmo = new Rfc2898DeriveBytes(Password, Salt, Iteraciones)) { 
                
                //Llave a validar
                var ChequearLlave = Algoritmo.GetBytes(PasswordConfiguration.KeySize);

                //Verificamos si la llave generada es igual a la guardada en la BD
                return ChequearLlave.SequenceEqual(Llave);
            }

        }
        #endregion
    }
}
