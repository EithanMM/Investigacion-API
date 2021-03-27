using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Rol.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class RolCore : ILecturaCore<RespuestaRolDTO>,
                           IEscrituraCore<RespuestaRolDTO, AgregarRolDTO, ActualizarRolDTO> {

        #region Atrobutos y constructor
        private static int RegistrosDefault = 5;
        private readonly ILecturaDataAccess<RolModel> ILecturaRol;
        private readonly IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> IEscrituraRol;

        public RolCore(ILecturaDataAccess<RolModel> RolLectura, IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> RolEscritura) {
            this.ILecturaRol = RolLectura;
            this.IEscrituraRol = RolEscritura;
        }
        #endregion

        #region Metodos
        public async Task<RespuestaRolDTO> Agregar(AgregarRolDTO Modelo) {

            ErrorModel Error;
            RespuestaRolDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraRol.Agregar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaRolDTO>(Resultado);
            return Respuesta;
        }

        public async Task<RespuestaRolDTO> Actualizar(ActualizarRolDTO Modelo) {

            ErrorModel Error;
            RespuestaRolDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraRol.Actualizar(Modelo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaRolDTO>(Resultado);
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaRolDTO>> Listar() {

            var Respuesta = await ILecturaRol.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaRolDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaRolDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaRolDTO>> ListarPaginacion(int NumeroPagina, int TamanoPagina) {

            if (NumeroPagina != 0) NumeroPagina = NumeroPagina - 1;
            if (TamanoPagina == 0) TamanoPagina = RegistrosDefault;

            var Respuesta = await ILecturaRol.ListarPaginacion(NumeroPagina * TamanoPagina, TamanoPagina);
            EntidadPaginacion<RespuestaRolDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaRolDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaRolDTO>.PaginarSQL(Objeto.Data, NumeroPagina, TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }

        public async Task<RespuestaRolDTO> Obtener(string Consecutivo) {

            ErrorModel Error;
            RespuestaRolDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturaRol.Obtener(Consecutivo);

            if (Resultado.Contains("Error")) {
                Error = Utf8Json.JsonSerializer.Deserialize<ErrorModel>(Resultado);
                throw new ExcepcionCore(Error);
            }

            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaRolDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El Rol con consecutivo " + Consecutivo + " no existe");
            return Respuesta;
        }
        #endregion
    }
}
