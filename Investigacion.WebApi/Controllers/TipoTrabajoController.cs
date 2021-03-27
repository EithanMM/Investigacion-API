using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.TipoTrabajo.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    [Produces("application/json")]
    [Route("api/v1/TipoTrabajo/[action]")]
    [ApiController]
    public class TipoTrabajoController : ControllerBase {

        #region Variables y constructor
        private readonly IEliminarCore IEliminarTipoTrabajo;
        private readonly ILecturaCore<TipoTrabajoModel> ILecturaTipoTrabajo;
        private readonly IEscrituraCore<TipoTrabajoModel, AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO> IEscrituraTipoTrabajo;

        public TipoTrabajoController(ILecturaCore<TipoTrabajoModel> TipoTrabajoLectura, IEscrituraCore<TipoTrabajoModel, AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO> TipoTrabajoEscritura, IEliminarCore TipoTrabajoEliminar) {
            this.ILecturaTipoTrabajo = TipoTrabajoLectura;
            this.IEliminarTipoTrabajo = TipoTrabajoEliminar;
            this.IEscrituraTipoTrabajo = TipoTrabajoEscritura;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de tipo trabajo 
        /// </summary>
        [HttpPost]
        [Authorize]
        [ActionName("Agregar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<TipoTrabajoModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarTipoTrabajoDTO Modelo) {

            TipoTrabajoModel Resultado = await IEscrituraTipoTrabajo.Agregar(Modelo);
            RespuestaApi<TipoTrabajoModel> Respuesta = new RespuestaApi<TipoTrabajoModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Actualiza un registro de tipo trabajo 
        /// </summary>
        [HttpPatch]
        [ActionName("Actualizar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<TipoTrabajoModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarTipoTrabajoDTO Modelo) {

            TipoTrabajoModel Resultado = await IEscrituraTipoTrabajo.Actualizar(Modelo);
            RespuestaApi<TipoTrabajoModel> Respuesta = new RespuestaApi<TipoTrabajoModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene un registro de tipo trabajo segun su consecutivo
        /// </summary>
        [HttpGet]
        [ActionName("Obtener")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<TipoTrabajoModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Obtener(string Consecutivo) {

            TipoTrabajoModel Resultado = await ILecturaTipoTrabajo.Obtener(Consecutivo);
            RespuestaApi<TipoTrabajoModel> Respuesta = new RespuestaApi<TipoTrabajoModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de tipo trabajo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Listar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<TipoTrabajoModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Listar() {

            IEnumerable<TipoTrabajoModel> Resultado = await ILecturaTipoTrabajo.Listar();
            RespuestaApi<IEnumerable<TipoTrabajoModel>> Respuesta = new RespuestaApi<IEnumerable<TipoTrabajoModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<TipoTrabajoModel>>))]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            Paginacion<TipoTrabajoModel> Resultado = await ILecturaTipoTrabajo.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<TipoTrabajoModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<TipoTrabajoModel>> Respuesta = new RespuestaApi<Paginacion<TipoTrabajoModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }

        [HttpDelete]
        [ActionName("Eliminar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Eliminar(string Consecutivo) {

            bool Resultado = await IEliminarTipoTrabajo.Eliminar(Consecutivo);
            var Respuesta = new RespuestaApi<bool>(Resultado);
            return Ok(Respuesta);
        }
        #endregion
    }
}
