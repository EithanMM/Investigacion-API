using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Rol.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Investigacion.WebApi.Controllers {

    [Produces("application/json")]
    [Route("api/v1/Rol/[action]")]
    [ApiController]
    public class RolController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<RolModel> ILecturaRol;
        private readonly IEscrituraCore<RolModel, AgregarRolDTO, ActualizarRolDTO> IEscrituraRol;

        public RolController(ILecturaCore<RolModel> RolLectura, IEscrituraCore<RolModel, AgregarRolDTO, ActualizarRolDTO> RolEscritura) {
            this.ILecturaRol = RolLectura;
            this.IEscrituraRol = RolEscritura;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de Rol 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RolModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarRolDTO Modelo) {

            RolModel Resultado = await IEscrituraRol.Agregar(Modelo);
            RespuestaApi<RolModel> Respuesta = new RespuestaApi<RolModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Actualiza un registro de rol
        /// </summary>
        [HttpPatch]
        [ActionName("Actualizar")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RolModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarRolDTO Modelo) {

            RolModel Resultado = await IEscrituraRol.Actualizar(Modelo);
            RespuestaApi<RolModel> Respuesta = new RespuestaApi<RolModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene un registro de rol segun su consecutivo
        /// </summary>
        [HttpGet]
        [ActionName("Obtener")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RolModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Obtener(string Consecutivo) {

            RolModel Resultado = await ILecturaRol.Obtener(Consecutivo);
            RespuestaApi<RolModel> Respuesta = new RespuestaApi<RolModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los roles.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<InvestigadorModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Listar() {

            IEnumerable<RolModel> Resultado = await ILecturaRol.Listar();
            RespuestaApi<IEnumerable<RolModel>> Respuesta = new RespuestaApi<IEnumerable<RolModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los roles segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<RolModel>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            var Resultado = await ILecturaRol.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<RolModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<RolModel>> Respuesta = new RespuestaApi<Paginacion<RolModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }
        #endregion
    }
}
