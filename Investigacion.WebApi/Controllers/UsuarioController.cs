using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Usuario.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    //[Authorize]
    [Produces("application/json")]
    [Route("api/v1/Usuario/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<UsuarioModel> ILecturaUsuario;
        private readonly IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO> IEscrituraUsuario;
        private readonly ISeguridadCore<ActualizarPasswordDTO, AccesoUsuarioDTO, RespuestaUsuarioDTO> ISeguridadUsuario;
        private readonly ITokenCore<UsuarioModel, RefreshTokenModel> IToken;

        public UsuarioController(ILecturaCore<UsuarioModel> UsuarioLectura, ISeguridadCore<ActualizarPasswordDTO, AccesoUsuarioDTO, RespuestaUsuarioDTO> UsuarioSeguridad, IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO> UsuarioEscritura, ITokenCore<UsuarioModel, RefreshTokenModel> IToken) {
            this.IToken = IToken;
            this.ILecturaUsuario = UsuarioLectura;
            this.ISeguridadUsuario = UsuarioSeguridad;
            this.IEscrituraUsuario = UsuarioEscritura;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de usuario 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<UsuarioModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarUsuarioDTO Modelo) {

            UsuarioModel Resultado = await IEscrituraUsuario.Agregar(Modelo);
            RespuestaApi<UsuarioModel> Respuesta = new RespuestaApi<UsuarioModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Autentica si el usuario existe o no.
        /// </summary>
        [HttpPost]
        [ActionName("Autenticar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RespuestaUsuarioDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Autenticar(AccesoUsuarioDTO Modelo) {

            RespuestaUsuarioDTO Respuesta = await ISeguridadUsuario.AutenticarUsuario(Modelo);
            HttpContext.Session.SetString("JWToken", Respuesta.Token);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Meotod para obtener un nuevo token
        /// </summary>
        [HttpGet]
        [ActionName("Refrescar token")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<string>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult ObtenerToken() {

            int ErrorCode = (int)HttpStatusCode.BadRequest;
            string JWToken = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(JWToken)) {
                var Decode = IToken.DecodificarToken(JWToken);
                string Fecha = IToken.ObtenerFechaToken(Decode);
                bool TokenExpirado = IToken.CompararFechas(Convert.ToDateTime(Fecha));

                if (TokenExpirado) {
                    string Token = IToken.GenerarToken(Decode);
                    return Ok(Token);
                }
                else return BadRequest(new { state = ErrorCode, error = "No hay token nuevo, el token actual sigue funcional." });
            }
            else return BadRequest(new { state = ErrorCode, error = "No existe usuario logeado en este momento." });
        }

        /// <summary>
        /// Obtiene los registros de los investigadores.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<UsuarioModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Listar() {

            IEnumerable<UsuarioModel> Resultado = await ILecturaUsuario.Listar();
            RespuestaApi<IEnumerable<UsuarioModel>> Respuesta = new RespuestaApi<IEnumerable<UsuarioModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<UsuarioModel>>))]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            var Resultado = await ILecturaUsuario.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<UsuarioModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<UsuarioModel>> Respuesta = new RespuestaApi<Paginacion<UsuarioModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }
        #endregion
    }
}
