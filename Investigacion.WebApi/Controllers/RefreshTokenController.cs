using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Investigacion.InterfaceCore;
using Investigacion.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investigacion.WebApi.Controllers {
    [Produces("application/json")]
    [Route("api/v1/RefreshToken/[action]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase {

        #region Variables y constructor
        private readonly ITokenCore<UsuarioModel, RefreshTokenModel> ITokenRefreshTokenCore;

        public RefreshTokenController(ITokenCore<UsuarioModel, RefreshTokenModel> ITokenRefreshTokenCore) {
            this.ITokenRefreshTokenCore = ITokenRefreshTokenCore;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de Ivestigador 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RefreshTokenModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GenerarTokenAcceso() {

            var Identity = HttpContext.User.Identity as ClaimsIdentity;

            RefreshTokenModel Resultado = await ITokenRefreshTokenCore.GenerarTokenAcceso(Identity);
            RespuestaApi<RefreshTokenModel> Respuesta = new RespuestaApi<RefreshTokenModel>(Resultado);
            return Created("Ok", Respuesta);
        }
        #endregion
    }
}
