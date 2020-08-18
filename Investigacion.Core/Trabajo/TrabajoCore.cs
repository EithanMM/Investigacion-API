
using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Trabajo.DTOModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class TrabajoCore : TrabajoInterfaceCore {

        #region Variables y constructor
        private readonly TrabajoInterfaceDataAccess Trabajo;
        private readonly TipoTrabajoInterfaceDataAccess TipoTrabajo;
        private readonly InvestigadorInterfaceDataAccess Investigador;
        private int PosicionMensajeError;

        public TrabajoCore(TrabajoInterfaceDataAccess Trabajo, TipoTrabajoInterfaceDataAccess TipoTrabajo, InvestigadorInterfaceDataAccess Investigador) {

            this.Trabajo = Trabajo;
            this.TipoTrabajo = TipoTrabajo;
            this.Investigador = Investigador;
            this.PosicionMensajeError = 6;
        }
        #endregion

        #region Metodos

        public async Task<TrabajoModel> Agregar(AgregarTrabajoDTO Modelo) {

            TrabajoModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");

            string InvestigadorJson = await Investigador.Obtener(Modelo.ConsecutivoInvestigador.ToUpper());
            string TipoTrabajoJson = await TipoTrabajo.Obtener(Modelo.ConsecutivoTipoTrabajo.ToUpper());

            if (InvestigadorJson.Equals("")) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Modelo.ConsecutivoInvestigador.ToUpper() + " no existe");
            else if (TipoTrabajoJson.Equals("")) throw new NotFoundExcepcionCore("El tipo de trabajo con consecutivo " + Modelo.ConsecutivoTipoTrabajo.ToUpper() + " no existe");

            Modelo.IdInvestigador = (Utf8Json.JsonSerializer.Deserialize<InvestigadorModel>(InvestigadorJson.Replace("LLP_", ""))).Id;
            Modelo.IdTipoTrabajo = (Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(TipoTrabajoJson.Replace("LLP_", ""))).Id;
            string Resultado = await Trabajo.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TrabajoModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));

            return Respuesta;
        }

        public async Task<TrabajoModel> Eliminar(string Consecutivo) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrabajoModel>> Listar() {

            string Resultado = await Trabajo.Listar();
            if (Resultado == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<TrabajoModel> Respuesta = Utf8Json.JsonSerializer.Deserialize<IEnumerable<TrabajoModel>>(Resultado);
            return Respuesta;
        }

        public async Task<TrabajoModel> Obtener(string Consecutivo) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
