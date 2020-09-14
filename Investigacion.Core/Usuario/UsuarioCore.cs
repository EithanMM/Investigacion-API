using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Usuario.DTOModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        private readonly ILecturaDataAccess<UsuarioModel> ILecturaUsuario;
        private readonly ISeguridadDataAccess<ActualizarPasswordDTO, AccesoUsuarioDTO> ISeguridadUsuario;
        private readonly IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO> IEscrituraUsuario;

        public UsuarioCore(ILecturaDataAccess<UsuarioModel> UsuarioLectura, ISeguridadDataAccess<ActualizarPasswordDTO, AccesoUsuarioDTO> UsuarioSeguridad, IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO> UsuarioEscritura, IConfiguration Configuracion) {
            this.Configuracion = Configuracion;
            this.ILecturaUsuario = UsuarioLectura;
            this.ISeguridadUsuario = UsuarioSeguridad;
            this.IEscrituraUsuario = UsuarioEscritura;
        }
        #endregion

        #region Metodos
        public async Task<UsuarioModel> Agregar(AgregarUsuarioDTO Modelo) {

            UsuarioModel Respuesta;
            AccesoUsuarioDTO Usuario;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");

            Usuario = new AccesoUsuarioDTO(Modelo.Usuario, Modelo.Password);
            string ExisteUsuario = await ISeguridadUsuario.AutenticarUsuario(Usuario);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<UsuarioModel>(ExisteUsuario);

            if (Respuesta == null) {
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
            RespuestaUsuarioDTO Resultado;

            var Respuesta = await ISeguridadUsuario.AutenticarUsuario(Modelo);
            UsuarioModel Usuario = Utf8Json.JsonSerializer.Deserialize<UsuarioModel>(Respuesta);
            if (Usuario != null) {
                //Se procede a generar el token
                Resultado = new RespuestaUsuarioDTO("Bienvenido " + Usuario.Usuario);
                Resultado.Token = GenerarToken(Usuario);
            }
            else {
                Resultado = new RespuestaUsuarioDTO("Las credenciales brindadas no corresponden, verifique el usaurio o el password.");
            }
            return Resultado;
        }

        #endregion

        #region Metodos privados
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
                new Claim(ClaimTypes.Role, Usuario.RolModel.Consecutivo.ToString())
            };

            //Payload
            var Payload = new JwtPayload(
                Configuracion["Authentication:Issuer"],
                Configuracion["Authentication:Audience"],
                Claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(2)
            );

            //Signature
            var Token = new JwtSecurityToken(Header, Payload);

            //Serializar y generar token
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        #endregion
    }
}
