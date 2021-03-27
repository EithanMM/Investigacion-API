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
        private readonly ILecturaDataAccess<InformacionInvestigadorModel> ILecturaInformacionInvestigador;
        private readonly IEscrituraDataAccess<AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> IEscrituraInformacionInvestigador;

        public InformacionInvestigadorCore(ILecturaDataAccess<InformacionInvestigadorModel> ILecturaInformacionInvestigador, IEscrituraDataAccess<AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> IEscrituraInformacionInvestigador) {
            this.ILecturaInformacionInvestigador = ILecturaInformacionInvestigador;
            this.IEscrituraInformacionInvestigador = IEscrituraInformacionInvestigador;
        }
        #endregion

        #region Metodos
        public async Task<RespuestaInformacionInvestigadorDTO> Agregar(AgregarInformacionInvestigadorDTO Modelo) {

            ErrorModel Error;
            RespuestaInformacionInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInformacionInvestigador.Agregar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInformacionInvestigadorDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaInformacionInvestigadorDTO> Actualizar(ActualizarInformacionInvestigadorDTO Modelo) {

            ErrorModel Error;
            RespuestaInformacionInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInformacionInvestigador.Actualizar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInformacionInvestigadorDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaInformacionInvestigadorDTO> Obtener(string Consecutivo) {

            ErrorModel Error;
            RespuestaInformacionInvestigadorDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturaInformacionInvestigador.Obtener(Consecutivo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInformacionInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("La informacion de investigador con consecutivo " + Consecutivo + " no existe");
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaInformacionInvestigadorDTO>> Listar() {

            var Respuesta = await ILecturaInformacionInvestigador.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaInformacionInvestigadorDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaInformacionInvestigadorDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaInformacionInvestigadorDTO>> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            NumeroPagina = NumeroPagina - 1;
            var Respuesta = await ILecturaInformacionInvestigador.ListarPaginacion(NumeroPagina * TamanoPagina, TamanoPagina);
            EntidadPaginacion<RespuestaInformacionInvestigadorDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaInformacionInvestigadorDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaInformacionInvestigadorDTO>.PaginarSQL(Objeto.Data, NumeroPagina, TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }
        #endregion
    }
}
