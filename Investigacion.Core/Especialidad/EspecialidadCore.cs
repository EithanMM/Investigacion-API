using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Especialidad.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class EspecialidadCore : ILecturaCore<EspecialidadModel>,
                                     IEscrituraCore<EspecialidadModel, AgregarEspecialidadDTO, ActualizarEspecialidadDTO>  {

        #region Variables y constructor
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;
        private readonly ILecturaDataAccess<EspecialidadModel> ILecturaEspecialidad;
        private readonly IEscrituraDataAccess<AgregarEspecialidadDTO, ActualizarEspecialidadDTO> IEscrituraEspecialidad;

        public EspecialidadCore(ILecturaDataAccess<EspecialidadModel> ILecturaEspecialidad, IEscrituraDataAccess<AgregarEspecialidadDTO, ActualizarEspecialidadDTO> IEscrituraEspecialidad) {
            this.ILecturaEspecialidad = ILecturaEspecialidad;
            this.IEscrituraEspecialidad = IEscrituraEspecialidad;
        }
        #endregion

        #region Metodos
        public async Task<EspecialidadModel> Agregar(AgregarEspecialidadDTO Modelo) {

            EspecialidadModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraEspecialidad.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<EspecialidadModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<EspecialidadModel> Actualizar(ActualizarEspecialidadDTO Modelo) {

            EspecialidadModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraEspecialidad.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<EspecialidadModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<EspecialidadModel> Obtener(string Consecutivo) {

            EspecialidadModel Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturaEspecialidad.Obtener(Consecutivo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<EspecialidadModel>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Consecutivo + " no existe");
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }
        public async Task<IEnumerable<EspecialidadModel>> Listar() {

            var Respuesta = await ILecturaEspecialidad.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<EspecialidadModel> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<EspecialidadModel>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<EspecialidadModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = ((int)NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = ((int)TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; 
            NumeroPagina = ((int)NumeroPagina - 1);

            var Respuesta = await ILecturaEspecialidad.ListarPaginacion((int)NumeroPagina * (int)TamanoPagina, (int)TamanoPagina);
            EntidadPaginacion<EspecialidadModel> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<EspecialidadModel>>(Respuesta);
            var RespuestaPaginada = Paginacion<EspecialidadModel>.PaginarSQL(Objeto.Data, (int)NumeroPagina, (int)TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }
        #endregion

    }
}
