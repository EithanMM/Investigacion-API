using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Especialidad.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class EspecialidadCore : ILecturaCore<RespuestaEspecialidadDTO>,
                                     IEscrituraCore<RespuestaEspecialidadDTO, AgregarEspecialidadDTO, ActualizarEspecialidadDTO>  {

        #region Variables y constructor
        private static int RegistrosDefault = 5;
        private readonly ILecturaDataAccess<EspecialidadModel> ILecturaEspecialidad;
        private readonly IEscrituraDataAccess<AgregarEspecialidadDTO, ActualizarEspecialidadDTO> IEscrituraEspecialidad;

        public EspecialidadCore(ILecturaDataAccess<EspecialidadModel> ILecturaEspecialidad, IEscrituraDataAccess<AgregarEspecialidadDTO, ActualizarEspecialidadDTO> IEscrituraEspecialidad) {
            this.ILecturaEspecialidad = ILecturaEspecialidad;
            this.IEscrituraEspecialidad = IEscrituraEspecialidad;
        }
        #endregion

        #region Metodos
        public async Task<RespuestaEspecialidadDTO> Agregar(AgregarEspecialidadDTO Modelo) {

            ErrorModel Error;
            RespuestaEspecialidadDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraEspecialidad.Agregar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaEspecialidadDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaEspecialidadDTO> Actualizar(ActualizarEspecialidadDTO Modelo) {

            ErrorModel Error;
            RespuestaEspecialidadDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraEspecialidad.Actualizar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaEspecialidadDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaEspecialidadDTO> Obtener(string Consecutivo) {

            ErrorModel Error;
            RespuestaEspecialidadDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturaEspecialidad.Obtener(Consecutivo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaEspecialidadDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El investigador con consecutivo " + Consecutivo + " no existe");
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaEspecialidadDTO>> Listar() {

            var Respuesta = await ILecturaEspecialidad.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaEspecialidadDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaEspecialidadDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaEspecialidadDTO>> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            if (NumeroPagina != 0) NumeroPagina = NumeroPagina - 1;
            if (TamanoPagina == 0) TamanoPagina = RegistrosDefault;

            var Respuesta = await ILecturaEspecialidad.ListarPaginacion(NumeroPagina * TamanoPagina, TamanoPagina);
            EntidadPaginacion<RespuestaEspecialidadDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaEspecialidadDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaEspecialidadDTO>.PaginarSQL(Objeto.Data, NumeroPagina, TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }
        #endregion

    }
}
