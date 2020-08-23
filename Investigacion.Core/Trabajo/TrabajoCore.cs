
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
    public class TrabajoCore : ILecturaCore<TrabajoModel>,
                               IEscrituraCore<TrabajoModel, AgregarTrabajoDTO, ActualizarTrabajoDTO> {

        #region Variables y constructor
        private readonly ILecturaDataAccess<TrabajoModel> ILecturaTrabajo;
        private readonly ILecturaDataAccess<TipoTrabajoModel> ILecturaTipoTrabajo;
        private readonly ILecturaDataAccess<InvestigadorModel> ILecturaInvestigador;
        private readonly IEscrituraDataAccess<AgregarTrabajoDTO, ActualizarTrabajoDTO> IEscrituraTrabajo;
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;

        public TrabajoCore(ILecturaDataAccess<TrabajoModel> ILecturaTrabajo, ILecturaDataAccess<TipoTrabajoModel> ILecturaTipoTrabajo, ILecturaDataAccess<InvestigadorModel> ILecturaInvestigador, IEscrituraDataAccess<AgregarTrabajoDTO, ActualizarTrabajoDTO> IEscrituraTrabajo) {
            this.ILecturaTrabajo = ILecturaTrabajo;
            this.IEscrituraTrabajo = IEscrituraTrabajo;
            this.ILecturaTipoTrabajo = ILecturaTipoTrabajo;
            this.ILecturaInvestigador = ILecturaInvestigador;
        }
        #endregion

        #region Metodos

        public async Task<TrabajoModel> Agregar(AgregarTrabajoDTO Modelo) {

            TrabajoModel Respuesta = new TrabajoModel();

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");

            string InvestigadorJson = await ILecturaInvestigador.Obtener(Modelo.ConsecutivoInvestigador.ToUpper());
            string TipoTrabajoJson = await ILecturaTipoTrabajo.Obtener(Modelo.ConsecutivoTipoTrabajo.ToUpper());

            if (InvestigadorJson.Equals("")) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Modelo.ConsecutivoInvestigador.ToUpper() + " no existe");
            else if (TipoTrabajoJson.Equals("")) throw new NotFoundExcepcionCore("El tipo de trabajo con consecutivo " + Modelo.ConsecutivoTipoTrabajo.ToUpper() + " no existe");

            Modelo.IdInvestigador = (Utf8Json.JsonSerializer.Deserialize<InvestigadorModel>(InvestigadorJson.Replace("LLP_", ""))).Id;
            Modelo.IdTipoTrabajo = (Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(TipoTrabajoJson.Replace("LLP_", ""))).Id;
            string Resultado = await IEscrituraTrabajo.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TrabajoModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));

            return Respuesta;
        }

        public Task<TrabajoModel> Actualizar(ActualizarTrabajoDTO Modelo) {
            throw new NotImplementedException();
        }

        public async Task<TrabajoModel> Obtener(string Consecutivo) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrabajoModel>> Listar() {

            string Resultado = await ILecturaTrabajo.Listar();
            if (Resultado == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<TrabajoModel> Respuesta = Utf8Json.JsonSerializer.Deserialize<IEnumerable<TrabajoModel>>(Resultado);
            return Respuesta;
        }

        public async Task<Paginacion<TrabajoModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {
            NumeroPagina = (NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = (TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; 

            string Resultado = await ILecturaTrabajo.Listar();
            var RespuestaPaginada = Paginacion<TrabajoModel>.Paginar(
                Utf8Json.JsonSerializer.Deserialize<IEnumerable<TrabajoModel>>(Resultado),
                (int)NumeroPagina, (int)TamanoPagina);

            return RespuestaPaginada;
        }
        #endregion
    }
}
