using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Usuario.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    //[Authorize]
    [Produces("application/json")]
    [Route("api/v1/Usuario/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<UsuarioModel> UsuarioLectura;
        private readonly ISeguridadCore<ActualizarPasswordDTO> UsuarioSeguridad;
        private readonly IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO> UsuarioEscritura;

        public UsuarioController(ILecturaCore<UsuarioModel> UsuarioLectura, ISeguridadCore<ActualizarPasswordDTO> UsuarioSeguridad, IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO> UsuarioEscritura) {
            this.UsuarioLectura = UsuarioLectura;
            this.UsuarioSeguridad = UsuarioSeguridad;
            this.UsuarioEscritura = UsuarioEscritura;
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

            UsuarioModel Resultado = await UsuarioEscritura.Agregar(Modelo);
            RespuestaApi<UsuarioModel> Respuesta = new RespuestaApi<UsuarioModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<UsuarioModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Listar() {

            IEnumerable<UsuarioModel> Resultado = await UsuarioLectura.Listar();
            RespuestaApi<IEnumerable<UsuarioModel>> Respuesta = new RespuestaApi<IEnumerable<UsuarioModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<UsuarioModel>>))]
        public async Task<IActionResult> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            var Resultado = await UsuarioLectura.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<UsuarioModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<UsuarioModel>> Respuesta = new RespuestaApi<Paginacion<UsuarioModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }
        #endregion
    }
}
