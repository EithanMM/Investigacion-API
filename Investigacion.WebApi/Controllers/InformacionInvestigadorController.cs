using Investigacion.InterfaceCore;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.InformacionInvestigador.DTOModels;
using Investigacion.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Investigacion.WebApi.Controllers {
    [Produces("application/json")]
    [Route("api/v1/InformacionInvestigador/[action]")]
    [ApiController]
    public class InformacionInvestigadorController : ControllerBase {

        #region Variables y constructor
        private readonly ILecturaCore<InformacionInvestigadorModel> ILecturaInformacionInvestigador;
        private readonly IEscrituraCore<InformacionInvestigadorModel, AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> IEscrituraInformacionInvestigador;

        public InformacionInvestigadorController(ILecturaCore<InformacionInvestigadorModel> ILecturaInformacionInvestigador, IEscrituraCore<InformacionInvestigadorModel, AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> IEscrituraInformacionInvestigador) {
            this.ILecturaInformacionInvestigador = ILecturaInformacionInvestigador;
            this.IEscrituraInformacionInvestigador = IEscrituraInformacionInvestigador;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Agrega un registro de InformacionInvestigador 
        /// </summary>
        [HttpPost]
        [ActionName("Agregar")]
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<InformacionInvestigadorModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Agregar([FromBody] AgregarInformacionInvestigadorDTO Modelo) {

            InformacionInvestigadorModel Resultado = await IEscrituraInformacionInvestigador.Agregar(Modelo);
            RespuestaApi<InformacionInvestigadorModel> Respuesta = new RespuestaApi<InformacionInvestigadorModel>(Resultado);
            return Created("Ok", Respuesta);
        }

        /// <summary>
        /// Actualiza un registro de InformacionInvestigador
        /// </summary>
        [HttpPatch]
        [ActionName("Actualizar")]
        [Authorize(Roles = "Administrador, Gestor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<InformacionInvestigadorModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarInformacionInvestigadorDTO Modelo) {

            InformacionInvestigadorModel Resultado = await IEscrituraInformacionInvestigador.Actualizar(Modelo);
            RespuestaApi<InformacionInvestigadorModel> Respuesta = new RespuestaApi<InformacionInvestigadorModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene un registro de InformacionInvestigador segun su consecutivo
        /// </summary>
        [HttpGet]
        [ActionName("Obtener")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<InformacionInvestigadorModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Obtener(string Consecutivo) {

            InformacionInvestigadorModel Resultado = await ILecturaInformacionInvestigador.Obtener(Consecutivo);
            RespuestaApi<InformacionInvestigadorModel> Respuesta = new RespuestaApi<InformacionInvestigadorModel>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de InformacionInvestigador.
        /// </summary>
        [HttpGet]
        [ActionName("Listar")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<IEnumerable<InformacionInvestigadorModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Listar() {

            IEnumerable<InformacionInvestigadorModel> Resultado = await ILecturaInformacionInvestigador.Listar();
            RespuestaApi<IEnumerable<InformacionInvestigadorModel>> Respuesta = new RespuestaApi<IEnumerable<InformacionInvestigadorModel>>(Resultado);
            return Ok(Respuesta);
        }

        /// <summary>
        /// Obtiene los registros de InformacionInvestigador segun su paginacion
        /// </summary>
        [HttpGet]
        [ActionName("ListarPaginacion")]
        [Authorize(Roles = "Administrador, Gestor, Invitado")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RespuestaApi<Paginacion<InformacionInvestigadorModel>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            var Resultado = await ILecturaInformacionInvestigador.ListarPaginacion(NumeroPagina, TamanoPagina);
            Metadata MetaData = PaginationHelper<InformacionInvestigadorModel>.SetMetaData(Resultado);
            RespuestaApi<Paginacion<InformacionInvestigadorModel>> Respuesta = new RespuestaApi<Paginacion<InformacionInvestigadorModel>>(Resultado) { Meta = MetaData };
            return Ok(Respuesta);
            //Response.Headers.Add("X-Pagination", Utf8Json.JsonSerializer.ToJsonString(MetaData));
        }
        #endregion
    }
}
