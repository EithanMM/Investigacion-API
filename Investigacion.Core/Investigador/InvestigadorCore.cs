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
    public class InvestigadorCore : ILecturaCore<InvestigadorModel>, 
                                    IEscrituraCore<InvestigadorModel, AgregarInvestigadorDTO, ActualizarInvestigadorDTO>, 
                                    IEliminarCore<InvestigadorModel> {

        #region Variables y constructor

        private readonly ILecturaDataAccess<InvestigadorModel> ILecturanvestigador;
        private readonly IEliminarDataAccess<InvestigadorModel> IEliminarInvestigador;
        private readonly IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO> IEscrituraInestigador;
        private static int PosicionMensajeError = 6;
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;


        public InvestigadorCore(ILecturaDataAccess<InvestigadorModel> ILecturanvestigador, IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO> IEscrituraInestigador, IEliminarDataAccess<InvestigadorModel> IEliminarInvestigador) {
            this.ILecturanvestigador = ILecturanvestigador;
            this.IEscrituraInestigador = IEscrituraInestigador;
            this.IEliminarInvestigador = IEliminarInvestigador;
        }
        #endregion

        #region Metodos

        public async Task<InvestigadorModel> Agregar(AgregarInvestigadorDTO Modelo) {

            InvestigadorModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInestigador.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<InvestigadorModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<InvestigadorModel> Actualizar(ActualizarInvestigadorDTO Modelo) {

            InvestigadorModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraInestigador.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<InvestigadorModel>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<IEnumerable<InvestigadorModel>> Listar() {

            var Respuesta = await ILecturanvestigador.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<InvestigadorModel> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<InvestigadorModel>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<InvestigadorModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = (NumeroPagina > 0) ? NumeroPagina : PaginaDefault;  // Si el numero de pagina menor o igual a 0, se setea en 1.
            TamanoPagina = (TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; // Si el tamano de pagina menor o igual a 0, se setea en 5.

            var Respuesta = await ILecturanvestigador.Listar();
            var RespuestaPaginada = Paginacion<InvestigadorModel>.Paginar(
                Utf8Json.JsonSerializer.Deserialize<IEnumerable<InvestigadorModel>>(Respuesta),
                (int)NumeroPagina, (int)TamanoPagina);

            return RespuestaPaginada;
        }

        public async Task<InvestigadorModel> Obtener(string Consecutivo) {

            InvestigadorModel Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturanvestigador.Obtener(Consecutivo.ToUpper());
            Respuesta = Utf8Json.JsonSerializer.Deserialize<InvestigadorModel>(Resultado);
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
