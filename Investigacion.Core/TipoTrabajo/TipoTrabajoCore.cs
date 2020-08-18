using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.TipoTrabajo.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class TipoTrabajoCore : TipoTrabajoInterfaceCore {

        #region Variables y cosntructor
        private readonly TipoTrabajoInterfaceDataAccess TipoTrabajo;
        private int PosicionMensajeError;
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;

        public TipoTrabajoCore(TipoTrabajoInterfaceDataAccess TipoTrabajo) {
            this.TipoTrabajo = TipoTrabajo;
            this.PosicionMensajeError = 6;

        }
        #endregion

        #region Metodos
        public async Task<TipoTrabajoModel> Agregar(AgregarTipoTrabajoDTO Modelo) {

            TipoTrabajoModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await TipoTrabajo.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<TipoTrabajoModel> Obtener(string Consecutivo) {

            TipoTrabajoModel Respuesta;

            if (Consecutivo == null) throw new ExcepcionCore("Debe digitar un consecutivo valido.");
            string Resultado = await TipoTrabajo.Obtener(Consecutivo.ToUpper());
            if (Resultado.Equals("")) throw new NotFoundExcepcionCore("El tipo de trabajo con consecutivo " + Consecutivo.ToUpper() + " no existe.");
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(Resultado);
            return Respuesta;
        }

        public async Task<TipoTrabajoModel> Actualizar(ActualizarTipoTrabajoDTO Modelo) {

            TipoTrabajoModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await TipoTrabajo.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<IEnumerable<TipoTrabajoModel>> Listar() {

            var Respuesta = await TipoTrabajo.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<TipoTrabajoModel> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<TipoTrabajoModel>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<TipoTrabajoModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = (NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = (TamanoPagina > 0) ? TamanoPagina : RegistrosDefault;

            var Respuesta = await TipoTrabajo.Listar();
            var RespuestaPaginada = Paginacion<TipoTrabajoModel>.Paginar(
                Utf8Json.JsonSerializer.Deserialize<IEnumerable<TipoTrabajoModel>>(Respuesta),
                (int)NumeroPagina, (int)TamanoPagina);

            return RespuestaPaginada;
        }

        public async Task<bool> Eliminar(string Consecutivo) {

            if (Consecutivo == null) throw new ExcepcionCore("No introdujo el consecutivo.");
            bool Resultado = await TipoTrabajo.Eliminar(Consecutivo.ToUpper());
            if (!Resultado) throw new NotFoundExcepcionCore("El registro " + Consecutivo.ToUpper() + " no existe o ya fue eliminado.");
            return Resultado;
        }
        #endregion
    }
}
