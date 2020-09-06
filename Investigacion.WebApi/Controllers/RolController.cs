using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.Rol.DTOModels;
using Microsoft.AspNetCore.Mvc;

namespace Investigacion.WebApi.Controllers {

    [Produces("application/json")]
    [Route("api/v1/Rol/[action]")]
    [ApiController]
    public class RolController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<RolModel> RolLectura;
        private readonly IEscrituraCore<RolModel, AgregarRolDTO, ActualizarRolDTO> RolEscritura;

        public RolController(ILecturaCore<RolModel> RolLectura, IEscrituraCore<RolModel, AgregarRolDTO, ActualizarRolDTO> RolEscritura) {
            this.RolLectura = RolLectura;
            this.RolEscritura = RolEscritura;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de Rol 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RolModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarRolDTO Modelo) {

            RolModel Resultado = await RolEscritura.Agregar(Modelo);
            RespuestaApi<RolModel> Respuesta = new RespuestaApi<RolModel>(Resultado);
            return Created("Ok", Respuesta);
        }
        #endregion
    }
}
