using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.TipoTrabajo.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class TipoTrabajoCore : ILecturaCore<RespuestaTipoTrabajoDTO>,
                                   IEscrituraCore<RespuestaTipoTrabajoDTO, AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO>,
                                   IEliminarCore {

        #region Variables y cosntructor
        private static int RegistrosDefault = 5;
        private readonly IEliminarDataAccess IEliminarTipoTrabajo;
        private readonly ILecturaDataAccess<TipoTrabajoModel> ILecturaTipoTrabajo;
        private readonly IEscrituraDataAccess<AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO> IEscrituraTipoTrabajo;

        public TipoTrabajoCore(ILecturaDataAccess<TipoTrabajoModel> TipoTrabajoLectura, IEscrituraDataAccess<AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO> TipoTrabajoLecturaEscritura, IEliminarDataAccess TipoTrabajoLecturaEliminar) {
            this.ILecturaTipoTrabajo = TipoTrabajoLectura;
            this.IEliminarTipoTrabajo = TipoTrabajoLecturaEliminar;
            this.IEscrituraTipoTrabajo = TipoTrabajoLecturaEscritura;
        }
        #endregion

        #region Metodos
        public async Task<RespuestaTipoTrabajoDTO> Agregar(AgregarTipoTrabajoDTO Modelo) {

            ErrorModel Error;
            RespuestaTipoTrabajoDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraTipoTrabajo.Agregar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaTipoTrabajoDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaTipoTrabajoDTO> Actualizar(ActualizarTipoTrabajoDTO Modelo) {

            ErrorModel Error;
            RespuestaTipoTrabajoDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraTipoTrabajo.Actualizar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaTipoTrabajoDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaTipoTrabajoDTO> Obtener(string Consecutivo) {

            ErrorModel Error;
            RespuestaTipoTrabajoDTO Respuesta;

            if (Consecutivo == null) throw new ExcepcionCore("Debe digitar un consecutivo valido.");
            string Resultado = await ILecturaTipoTrabajo.Obtener(Consecutivo.ToUpper());

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaTipoTrabajoDTO>(Resultado);
            if (Resultado.Equals("")) throw new NotFoundExcepcionCore("El tipo de trabajo con consecutivo " + Consecutivo.ToUpper() + " no existe.");
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaTipoTrabajoDTO>> Listar() {

            var Respuesta = await ILecturaTipoTrabajo.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaTipoTrabajoDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaTipoTrabajoDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaTipoTrabajoDTO>> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            if (NumeroPagina != 0) NumeroPagina = NumeroPagina - 1;
            if (TamanoPagina == 0) TamanoPagina = RegistrosDefault;

            var Respuesta = await ILecturaTipoTrabajo.ListarPaginacion(NumeroPagina * TamanoPagina, TamanoPagina);
            EntidadPaginacion<RespuestaTipoTrabajoDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaTipoTrabajoDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaTipoTrabajoDTO>.PaginarSQL(Objeto.Data, NumeroPagina, TamanoPagina, Objeto.Total);

            return RespuestaPaginada;
        }

        public async Task<bool> Eliminar(string Consecutivo) {

            if (Consecutivo == null) throw new ExcepcionCore("No introdujo el consecutivo.");
            bool Resultado = await IEliminarTipoTrabajo.Eliminar(Consecutivo.ToUpper());
            if (!Resultado) throw new NotFoundExcepcionCore("El registro " + Consecutivo.ToUpper() + " no existe o ya fue eliminado.");
            return Resultado;
        }
        #endregion
    }
}
