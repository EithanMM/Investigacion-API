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
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;
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

            RespuestaInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInestigador.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<RespuestaInvestigadorDTO> Actualizar(ActualizarInvestigadorDTO Modelo) {

            RespuestaInvestigadorDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInestigador.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInvestigadorDTO>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaInvestigadorDTO>> Listar() {

            var Respuesta = await ILecturanvestigador.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaInvestigadorDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaInvestigadorDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaInvestigadorDTO>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = ((int)NumeroPagina > 0) ? NumeroPagina : PaginaDefault;  // Si el numero de pagina menor o igual a 0, se setea en 1.
            TamanoPagina = ((int)TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; // Si el tamano de pagina menor o igual a 0, se setea en 5.
            NumeroPagina = ((int)NumeroPagina - 1);

            var Respuesta = await ILecturanvestigador.ListarPaginacion((int)NumeroPagina * (int)TamanoPagina, (int)TamanoPagina);
            EntidadPaginacion<RespuestaInvestigadorDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaInvestigadorDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaInvestigadorDTO>.PaginarSQL(Objeto.Data, (int)NumeroPagina, (int)TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }

        public async Task<RespuestaInvestigadorDTO> Obtener(string Consecutivo) {

            RespuestaInvestigadorDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturanvestigador.Obtener(Consecutivo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaInvestigadorDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Consecutivo + " no existe");
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
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
