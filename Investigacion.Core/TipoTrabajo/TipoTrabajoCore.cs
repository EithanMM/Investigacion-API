using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.TipoTrabajo.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class TipoTrabajoCore : ILecturaCore<TipoTrabajoModel>,
                                   IEscrituraCore<TipoTrabajoModel, AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO>,
                                   IEliminarCore<TipoTrabajoModel> {

        #region Variables y cosntructor
        private readonly ILecturaDataAccess<TipoTrabajoModel> TipoTrabajoLectura;
        private readonly IEliminarDataAccess<TipoTrabajoModel> TipoTrabajoLecturaEliminar;
        private readonly IEscrituraDataAccess<AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO> TipoTrabajoLecturaEscritura;
        private static int PosicionMensajeError = 6;
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;

        public TipoTrabajoCore(ILecturaDataAccess<TipoTrabajoModel> TipoTrabajoLectura, IEscrituraDataAccess<AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO> TipoTrabajoLecturaEscritura, IEliminarDataAccess<TipoTrabajoModel> TipoTrabajoLecturaEliminar) {
            this.TipoTrabajoLectura = TipoTrabajoLectura;
            this.TipoTrabajoLecturaEliminar = TipoTrabajoLecturaEliminar;
            this.TipoTrabajoLecturaEscritura = TipoTrabajoLecturaEscritura;
        }
        #endregion

        #region Metodos
        public async Task<TipoTrabajoModel> Agregar(AgregarTipoTrabajoDTO Modelo) {

            TipoTrabajoModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await TipoTrabajoLecturaEscritura.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<TipoTrabajoModel> Actualizar(ActualizarTipoTrabajoDTO Modelo) {

            TipoTrabajoModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await TipoTrabajoLecturaEscritura.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<TipoTrabajoModel> Obtener(string Consecutivo) {

            TipoTrabajoModel Respuesta;

            if (Consecutivo == null) throw new ExcepcionCore("Debe digitar un consecutivo valido.");
            string Resultado = await TipoTrabajoLectura.Obtener(Consecutivo.ToUpper());
            if (Resultado.Equals("")) throw new NotFoundExcepcionCore("El tipo de trabajo con consecutivo " + Consecutivo.ToUpper() + " no existe.");
            Respuesta = Utf8Json.JsonSerializer.Deserialize<TipoTrabajoModel>(Resultado);
            return Respuesta;
        }

        public async Task<IEnumerable<TipoTrabajoModel>> Listar() {

            var Respuesta = await TipoTrabajoLectura.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<TipoTrabajoModel> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<TipoTrabajoModel>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<TipoTrabajoModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = (NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = (TamanoPagina > 0) ? TamanoPagina : RegistrosDefault;

            var Respuesta = await TipoTrabajoLectura.Listar();
            var RespuestaPaginada = Paginacion<TipoTrabajoModel>.Paginar(
                Utf8Json.JsonSerializer.Deserialize<IEnumerable<TipoTrabajoModel>>(Respuesta),
                (int)NumeroPagina, (int)TamanoPagina);

            return RespuestaPaginada;
        }

        public async Task<bool> Eliminar(string Consecutivo) {

            if (Consecutivo == null) throw new ExcepcionCore("No introdujo el consecutivo.");
            bool Resultado = await TipoTrabajoLecturaEliminar.Eliminar(Consecutivo.ToUpper());
            if (!Resultado) throw new NotFoundExcepcionCore("El registro " + Consecutivo.ToUpper() + " no existe o ya fue eliminado.");
            return Resultado;
        }
        #endregion
    }
}
