using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Trabajo.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    [Produces("application/json")]
    [Route("api/v1/Trabajo/[action]")]
    [ApiController]
    public class TrabajoController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<TrabajoModel> ILecturaTrabajo;
        private readonly IEscrituraCore<TrabajoModel, AgregarTrabajoDTO, ActualizarTrabajoDTO> IEscrituraTrabajo;

        public TrabajoController(ILecturaCore<TrabajoModel> TrabajoLectura, IEscrituraCore<TrabajoModel, AgregarTrabajoDTO, ActualizarTrabajoDTO> TrabajoLecturaEscritura) {
            this.ILecturaTrabajo = TrabajoLectura;
            this.IEscrituraTrabajo = TrabajoLecturaEscritura;
        }
        #endregion

        #region Endpoints
        [HttpPost]
        [ActionName("Agregar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<TrabajoModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarTrabajoDTO Modelo) {

            TrabajoModel Resultado = await IEscrituraTrabajo.Agregar(Modelo);
            RespuestaApi<TrabajoModel> Respuesta = new RespuestaApi<TrabajoModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los trabajos.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<TrabajoModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Listar() {

            IEnumerable<TrabajoModel> Resultado = await ILecturaTrabajo.Listar();
            RespuestaApi<IEnumerable<TrabajoModel>> Respuesta = new RespuestaApi<IEnumerable<TrabajoModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los trabajos. segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<TrabajoModel>>))]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            var Resultado = await ILecturaTrabajo.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<TrabajoModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<TrabajoModel>> Respuesta = new RespuestaApi<Paginacion<TrabajoModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }
        #endregion
    }
}
