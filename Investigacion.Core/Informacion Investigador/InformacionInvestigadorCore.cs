using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.InformacionInvestigador.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class InformacionInvestigadorCore : ILecturaCore<RespuestaInformacionInvestigadorDTO>,
                                               IEscrituraCore<RespuestaInformacionInvestigadorDTO, AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> { 

        #region Variables y constructor
    private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;
        private readonly ILecturaDataAccess<InformacionInvestigadorModel> ILecturaInformacionInvestigador;
        private readonly IEscrituraDataAccess<AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> IEscrituraInformacionInvestigador;

        public InformacionInvestigadorCore(ILecturaDataAccess<InformacionInvestigadorModel> ILecturaInformacionInvestigador, IEscrituraDataAccess<AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> IEscrituraInformacionInvestigador) {
            this.ILecturaInformacionInvestigador = ILecturaInformacionInvestigador;
            this.IEscrituraInformacionInvestigador = IEscrituraInformacionInvestigador;
        }
        #endregion

        #region Metodos
        public async Task<RespuestaInformacionInvestigadorDTO> Agregar(AgregarInformacionInvestigadorDTO Modelo) {

            RespuestaInformacionInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInformacionInvestigador.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInformacionInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<RespuestaInformacionInvestigadorDTO> Actualizar(ActualizarInformacionInvestigadorDTO Modelo) {

            RespuestaInformacionInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInformacionInvestigador.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInformacionInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<RespuestaInformacionInvestigadorDTO> Obtener(string Consecutivo) {

            RespuestaInformacionInvestigadorDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturaInformacionInvestigador.Obtener(Consecutivo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInformacionInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("La informacion de investigador con consecutivo " + Consecutivo + " no existe");
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaInformacionInvestigadorDTO>> Listar() {

            var Respuesta = await ILecturaInformacionInvestigador.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaInformacionInvestigadorDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaInformacionInvestigadorDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaInformacionInvestigadorDTO>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {


            NumeroPagina = ((int)NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = ((int)TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; 
            NumeroPagina = ((int)NumeroPagina - 1);

            var Respuesta = await ILecturaInformacionInvestigador.ListarPaginacion((int)NumeroPagina * (int)TamanoPagina, (int)TamanoPagina);
            EntidadPaginacion<RespuestaInformacionInvestigadorDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaInformacionInvestigadorDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaInformacionInvestigadorDTO>.PaginarSQL(Objeto.Data, (int)NumeroPagina, (int)TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }
        #endregion
    }
}
