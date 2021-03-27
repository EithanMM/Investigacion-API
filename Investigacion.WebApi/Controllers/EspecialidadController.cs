using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Especialidad.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    [Produces("application/json")]
    [Route("api/v1/Especialidad/[action]")]
    [ApiController]
    public class EspecialidadController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<EspecialidadModel> ILecturaEspecialidad;
        private readonly IEscrituraCore<EspecialidadModel, AgregarEspecialidadDTO, ActualizarEspecialidadDTO> IEscrituraEspecialidad;

        public EspecialidadController(ILecturaCore<EspecialidadModel> ILecturaEspecialidad, IEscrituraCore<EspecialidadModel, AgregarEspecialidadDTO, ActualizarEspecialidadDTO> IEscrituraEspecialidad) {
            this.ILecturaEspecialidad = ILecturaEspecialidad;
            this.IEscrituraEspecialidad = IEscrituraEspecialidad;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de especialidad 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<EspecialidadModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarEspecialidadDTO Modelo) {

            EspecialidadModel Resultado = await IEscrituraEspecialidad.Agregar(Modelo);
            RespuestaApi<EspecialidadModel> Respuesta = new RespuestaApi<EspecialidadModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Actualiza un registro de especialidad
        /// </summary>
        [HttpPatch]
        [ActionName("Actualizar")]
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<EspecialidadModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarEspecialidadDTO Modelo) {

            EspecialidadModel Resultado = await IEscrituraEspecialidad.Actualizar(Modelo);
            RespuestaApi<EspecialidadModel> Respuesta = new RespuestaApi<EspecialidadModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene un registro de especialidad segun su consecutivo
        /// </summary>
        [HttpGet]
        [ActionName("Obtener")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<EspecialidadModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Obtener(string Consecutivo) {

            EspecialidadModel Resultado = await ILecturaEspecialidad.Obtener(Consecutivo);
            RespuestaApi<EspecialidadModel> Respuesta = new RespuestaApi<EspecialidadModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de las especialidades.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<EspecialidadModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Listar() {

            IEnumerable<EspecialidadModel> Resultado = await ILecturaEspecialidad.Listar();
            RespuestaApi<IEnumerable<EspecialidadModel>> Respuesta = new RespuestaApi<IEnumerable<EspecialidadModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de las especialidades segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<EspecialidadModel>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            var Resultado = await ILecturaEspecialidad.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<EspecialidadModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<EspecialidadModel>> Respuesta = new RespuestaApi<Paginacion<EspecialidadModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }
        #endregion
    }
}
