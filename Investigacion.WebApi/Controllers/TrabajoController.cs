using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.Trabajo.DTOModels;
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
        private readonly TrabajoInterfaceCore Trabajo;

        public TrabajoController(TrabajoInterfaceCore Trabajo) {
            this.Trabajo = Trabajo;
        }
        #endregion

        #region Endpoints
        [HttpPost]
        [ActionName("Agregar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<TrabajoModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarTrabajoDTO Modelo) {

            TrabajoModel Resultado = await Trabajo.Agregar(Modelo);
            RespuestaApi<TrabajoModel> Respuesta = new RespuestaApi<TrabajoModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los trabajos.
        /// </summary>
        [HttpGet]
        [ActionName("ObtenerTodos")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<TrabajoModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Listar() {

            IEnumerable<TrabajoModel> Resultado = await Trabajo.Listar();
            RespuestaApi<IEnumerable<TrabajoModel>> Respuesta = new RespuestaApi<IEnumerable<TrabajoModel>>(Resultado);
            return Ok(Respuesta);
        }
        #endregion
    }
}
