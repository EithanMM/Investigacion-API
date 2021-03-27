
using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Trabajo.DTOModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class TrabajoCore : ILecturaCore<RespuestaTrabajoDTO>,
                               IEscrituraCore<RespuestaTrabajoDTO, AgregarTrabajoDTO, ActualizarTrabajoDTO> {

        #region Variables y constructor
        private static int RegistrosDefault = 5;
        private readonly ILecturaDataAccess<TrabajoModel> ILecturaTrabajo;
        private readonly ILecturaDataAccess<TipoTrabajoModel> ILecturaTipoTrabajo;
        private readonly ILecturaDataAccess<InvestigadorModel> ILecturaInvestigador;
        private readonly IEscrituraDataAccess<AgregarTrabajoDTO, ActualizarTrabajoDTO> IEscrituraTrabajo;

        public TrabajoCore(ILecturaDataAccess<TrabajoModel> ILecturaTrabajo, ILecturaDataAccess<TipoTrabajoModel> ILecturaTipoTrabajo, ILecturaDataAccess<InvestigadorModel> ILecturaInvestigador, IEscrituraDataAccess<AgregarTrabajoDTO, ActualizarTrabajoDTO> IEscrituraTrabajo) {
            this.ILecturaTrabajo = ILecturaTrabajo;
            this.IEscrituraTrabajo = IEscrituraTrabajo;
            this.ILecturaTipoTrabajo = ILecturaTipoTrabajo;
            this.ILecturaInvestigador = ILecturaInvestigador;
        }
        #endregion

        #region Metodos

        public async Task<RespuestaTrabajoDTO> Agregar(AgregarTrabajoDTO Modelo) {

            ErrorModel Error;
            RespuestaTrabajoDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");

            string InvestigadorJson = await ILecturaInvestigador.Obtener(Modelo.ConsecutivoInvestigador.ToUpper());
            string TipoTrabajoJson = await ILecturaTipoTrabajo.Obtener(Modelo.ConsecutivoTipoTrabajo.ToUpper());

            if (InvestigadorJson.Equals("")) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Modelo.ConsecutivoInvestigador.ToUpper() + " no existe");
            else if (TipoTrabajoJson.Equals("")) throw new NotFoundExcepcionCore("El tipo de trabajo con consecutivo " + Modelo.ConsecutivoTipoTrabajo.ToUpper() + " no existe");

            Modelo.IdInvestigador = (Utf8Json.JsonSerializer.Deserialize<InvestigadorModel>(InvestigadorJson.Replace("LLP_", ""))).Id;
            Modelo.IdTipoTrabajo = (Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(TipoTrabajoJson.Replace("LLP_", ""))).Id;
            string Resultado = await IEscrituraTrabajo.Agregar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaTrabajoDTO>(Resultado);
            return Respuesta;
        }

        public Task<RespuestaTrabajoDTO> Actualizar(ActualizarTrabajoDTO Modelo) {
            throw new NotImplementedException();
        }

        public async Task<RespuestaTrabajoDTO> Obtener(string Consecutivo) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RespuestaTrabajoDTO>> Listar() {

            string Resultado = await ILecturaTrabajo.Listar();
            if (Resultado == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaTrabajoDTO> Respuesta = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaTrabajoDTO>>(Resultado);
            return Respuesta;
        }

        public async Task<Paginacion<RespuestaTrabajoDTO>> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            if(NumeroPagina != 0) NumeroPagina = NumeroPagina - 1;
            if (TamanoPagina == 0) TamanoPagina = RegistrosDefault;

            string Respuesta = await ILecturaTrabajo.ListarPaginacion(NumeroPagina * TamanoPagina, TamanoPagina);
            EntidadPaginacion<RespuestaTrabajoDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaTrabajoDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaTrabajoDTO>.PaginarSQL(Objeto.Data, NumeroPagina, TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }
        #endregion
    }
}
