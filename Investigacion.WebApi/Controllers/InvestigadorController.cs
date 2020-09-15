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
        private readonly ILecturaCore<InvestigadorModel> ILecturaInvestigador;
        private readonly IEliminarCore IEliminarInvestigador;
        private readonly IEscrituraCore<InvestigadorModel, AgregarInvestigadorDTO, ActualizarInvestigadorDTO> IEscrituraInvestigador;

        public InvestigadorController(ILecturaCore<InvestigadorModel> InvestigadorLectura, IEscrituraCore<InvestigadorModel, AgregarInvestigadorDTO, ActualizarInvestigadorDTO> InvestigadorEscritura, IEliminarCore InvestigadorEliminar) {
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
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<InvestigadorModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Agregar([FromBody] AgregarInvestigadorDTO Modelo) {

            InvestigadorModel Resultado = await IEscrituraInvestigador.Agregar(Modelo);
            RespuestaApi<InvestigadorModel> Respuesta = new RespuestaApi<InvestigadorModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Actualiza un registro de investigador
        /// </summary>
        [HttpPatch]
        [ActionName("Actualizar")]
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<InvestigadorModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarInvestigadorDTO Modelo) {

            InvestigadorModel Resultado = await IEscrituraInvestigador.Actualizar(Modelo);
            RespuestaApi<InvestigadorModel> Respuesta = new RespuestaApi<InvestigadorModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene un registro de investigador segun su consecutivo
        /// </summary>
        [HttpGet]
        [ActionName("Obtener")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<InvestigadorModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Obtener(string Consecutivo) {

            InvestigadorModel Resultado = await ILecturaInvestigador.Obtener(Consecutivo);
            RespuestaApi<InvestigadorModel> Respuesta = new RespuestaApi<InvestigadorModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<InvestigadorModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Listar() {

            IEnumerable<InvestigadorModel> Resultado = await ILecturaInvestigador.Listar();
            RespuestaApi<IEnumerable<InvestigadorModel>> Respuesta = new RespuestaApi<IEnumerable<InvestigadorModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de los investigadores segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<InvestigadorModel>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            var Resultado = await ILecturaInvestigador.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<InvestigadorModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<InvestigadorModel>> Respuesta = new RespuestaApi<Paginacion<InvestigadorModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }

        /// <summary>
        /// Elimina un registro de investigador segun su consecutivo
        /// </summary>
        [HttpDelete]
        [ActionName("Eliminar")]
        [Authorize(Roles = "Administrador, Gestor")]
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
