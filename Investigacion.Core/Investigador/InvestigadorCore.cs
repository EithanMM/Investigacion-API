using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Investigador.DTOModels;
using System.Collections.Generic;
//using System.Text.Json;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class InvestigadorCore : ILecturaCore<RespuestaInvestigadorDTO>, 
                                    IEscrituraCore<RespuestaInvestigadorDTO, AgregarInvestigadorDTO, ActualizarInvestigadorDTO>, 
                                    IEliminarCore {

        #region Variables y constructor
        private static int RegistrosDefault = 5;
        private readonly ILecturaDataAccess<InvestigadorModel> ILecturanvestigador;
        private readonly IEliminarDataAccess IEliminarInvestigador;
        private readonly IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO> IEscrituraInestigador;

        public InvestigadorCore(ILecturaDataAccess<InvestigadorModel> ILecturanvestigador, IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO> IEscrituraInestigador, IEliminarDataAccess IEliminarInvestigador) {
            this.ILecturanvestigador = ILecturanvestigador;
            this.IEscrituraInestigador = IEscrituraInestigador;
            this.IEliminarInvestigador = IEliminarInvestigador;
        }
        #endregion

        #region Metodos

        public async Task<RespuestaInvestigadorDTO> Agregar(AgregarInvestigadorDTO Modelo) {

            ErrorModel Error;
            RespuestaInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInestigador.Agregar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInvestigadorDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaInvestigadorDTO> Actualizar(ActualizarInvestigadorDTO Modelo) {

            ErrorModel Error;
            RespuestaInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInestigador.Actualizar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInvestigadorDTO>(Resultado);
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaInvestigadorDTO>> Listar() {

            var Respuesta = await ILecturanvestigador.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaInvestigadorDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaInvestigadorDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaInvestigadorDTO>> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            if (NumeroPagina != 0) NumeroPagina = NumeroPagina - 1;
            if (TamanoPagina == 0) TamanoPagina = RegistrosDefault;

            var Respuesta = await ILecturanvestigador.ListarPaginacion(NumeroPagina * TamanoPagina, TamanoPagina);
            EntidadPaginacion<RespuestaInvestigadorDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaInvestigadorDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaInvestigadorDTO>.PaginarSQL(Objeto.Data, NumeroPagina, TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }

        public async Task<RespuestaInvestigadorDTO> Obtener(string Consecutivo) {

            ErrorModel Error;
            RespuestaInvestigadorDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturanvestigador.Obtener(Consecutivo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Consecutivo + " no existe");
            return Respuesta;
        }

        public async Task<bool> Eliminar(string Consecutivo) {

            if (Consecutivo == null) throw new ExcepcionCore("No introdujo el consecutivo.");
            bool Resultado = await IEliminarInvestigador.Eliminar(Consecutivo.ToUpper());
            if (!Resultado) throw new NotFoundExcepcionCore("El registro " + Consecutivo.ToUpper() + " no existe o ya fue eliminado.");
            return Resultado;
        }
        #endregion
    }
}
