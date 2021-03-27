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
        [ActionName("Actualizar")]
        [HttpPatch(Name = "Actualizar")]
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
        [ActionName("Obtener")]
        [HttpGet("{Consecutivo}", Name = "Obtener")]
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
            return this.HATEOASResult(Resultado, (v) => this.Ok(new RespuestaApi<object>(v)));
        }

        /// <summary>
        /// Obtiene los registros de los investigadores segun su paginacion
        /// </summary>
        [HttpGet("{NumeroPagina}/{TamanoPagina}")]
        [ActionName("ListarPaginacion")]
        //[Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<RespuestaInvestigadorDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            var Resultado = await ILecturaInvestigador.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<RespuestaInvestigadorDTO>.SetMetaData(Resultado);
            return this.HATEOASResult(Resultado, (v) => this.Ok(new RespuestaApi<object>(v) { Meta = MetaData }));
        }

        /// <summary>
        /// Elimina un registro de investigador segun su consecutivo
        /// </summary>
        [ActionName("Eliminar")]
        [HttpDelete("{Consecutivo}", Name = "Eliminar")]
        //[Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Eliminar(string Consecutivo) {

            bool Resultado = await IEliminarInvestigador.Eliminar(Consecutivo);
            var Respuesta = new RespuestaApi<bool>(Resultado);
            return Ok(Respuesta);
        }
        #endregion
    }
}
