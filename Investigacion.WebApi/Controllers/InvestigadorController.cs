using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Investigador.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    //[Authorize]
    [Produces("application/json")]
    [Route("api/v1/Investigador/[action]")]
    [ApiController]
    public class InvestigadorController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<RespuestaInvestigadorDTO> ILecturaInvestigador;
        private readonly IEliminarCore IEliminarInvestigador;
        private readonly IEscrituraCore<RespuestaInvestigadorDTO, AgregarInvestigadorDTO, ActualizarInvestigadorDTO> IEscrituraInvestigador;

        public InvestigadorController(ILecturaCore<RespuestaInvestigadorDTO> InvestigadorLectura, IEscrituraCore<RespuestaInvestigadorDTO, AgregarInvestigadorDTO, ActualizarInvestigadorDTO> InvestigadorEscritura, IEliminarCore InvestigadorEliminar) {
            this.ILecturaInvestigador = InvestigadorLectura;
            this.IEliminarInvestigador = InvestigadorEliminar;
            this.IEscrituraInvestigador = InvestigadorEscritura;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de Ivestigador 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        //[Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RespuestaInvestigadorDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Agregar([FromBody] AgregarInvestigadorDTO Modelo) {

            RespuestaInvestigadorDTO Resultado = await IEscrituraInvestigador.Agregar(Modelo);
            return this.HATEOASResult(Resultado, (v) => this.Ok(new RespuestaApi<object>(v)));
        }

        /// <summary>
        /// Actualiza un registro de investigador
        /// </summary>
        [HttpPatch(Name = "Actualizar")]
        [ActionName("Actualizar")]
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RespuestaInvestigadorDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarInvestigadorDTO Modelo) {

            RespuestaInvestigadorDTO Resultado = await IEscrituraInvestigador.Actualizar(Modelo);
            return this.HATEOASResult(Resultado, (v) => this.Ok(new RespuestaApi<object>(v)));
        }

        /// <summary>
        /// Obtiene un registro de investigador segun su consecutivo
        /// </summary>
        [HttpGet(Name = "Obtener")]
        [ActionName("Obtener")]
        //[Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<RespuestaInvestigadorDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Obtener(string Consecutivo) {

            RespuestaInvestigadorDTO Resultado = await ILecturaInvestigador.Obtener(Consecutivo);
            return this.HATEOASResult(Resultado, (v) => this.Ok(new RespuestaApi<object>(v)));
        }

        /// <summary>
        /// Obtiene los registros de los investigadores.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        //[Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<RespuestaInvestigadorDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Listar() {

            IEnumerable<RespuestaInvestigadorDTO> Resultado = await ILecturaInvestigador.Listar();
            //RespuestaApi<IEnumerable<RespuestaInvestigadorDTO>> Respuesta = new RespuestaApi<IEnumerable<RespuestaInvestigadorDTO>>(Resultado);
            return this.HATEOASResult(Resultado, (v) => this.Ok(new RespuestaApi<object>(v)));
            //return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<RespuestaInvestigadorDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            var Resultado = await ILecturaInvestigador.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<RespuestaInvestigadorDTO>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<RespuestaInvestigadorDTO>> Respuesta = new RespuestaApi<Paginacion<RespuestaInvestigadorDTO>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }

        /// <summary>
        /// Elimina un registro de investigador segun su consecutivo
        /// </summary>
        [HttpDelete("{Consecutivo}", Name = "Eliminar")]
        [ActionName("Eliminar")]
        //[Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpDelete]
        public async Task<IActionResult> Eliminar(string Consecutivo) {

            bool Resultado = await IEliminarInvestigador.Eliminar(Consecutivo);
            var Respuesta = new RespuestaApi<bool>(Resultado);
            return Ok(Respuesta);
        }
        #endregion
    }
}
