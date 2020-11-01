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
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;
        private readonly ILecturaDataAccess<RolModel> ILecturaRol;
        private readonly IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> IEscrituraRol;

        public RolCore(ILecturaDataAccess<RolModel> RolLectura, IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> RolEscritura) {
            this.ILecturaRol = RolLectura;
            this.IEscrituraRol = RolEscritura;
        }
        #endregion

        #region Metodos
        public async Task<RespuestaRolDTO> Agregar(AgregarRolDTO Modelo) {

            RespuestaRolDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraRol.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaRolDTO>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<RespuestaRolDTO> Actualizar(ActualizarRolDTO Modelo) {

            RespuestaRolDTO Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraRol.Actualizar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaRolDTO>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public async Task<IEnumerable<RespuestaRolDTO>> Listar() {

            var Respuesta = await ILecturaRol.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<RespuestaRolDTO> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<RespuestaRolDTO>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<RespuestaRolDTO>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = ((int)NumeroPagina > 0) ? NumeroPagina : PaginaDefault;
            TamanoPagina = ((int)TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; 
            NumeroPagina = ((int)NumeroPagina - 1);

            var Respuesta = await ILecturaRol.ListarPaginacion((int)NumeroPagina * (int)TamanoPagina, (int)TamanoPagina);
            EntidadPaginacion<RespuestaRolDTO> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<RespuestaRolDTO>>(Respuesta);
            var RespuestaPaginada = Paginacion<RespuestaRolDTO>.PaginarSQL(Objeto.Data, (int)NumeroPagina, (int)TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }

        public async Task<RespuestaRolDTO> Obtener(string Consecutivo) {

            RespuestaRolDTO Respuesta;

            if (Consecutivo.Equals("")) throw new ExcepcionCore("No introdujo el consecutivo.");
            string Resultado = await ILecturaRol.Obtener(Consecutivo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RespuestaRolDTO>(Resultado);
            if (Respuesta == null) throw new NotFoundExcepcionCore("El Rol con consecutivo " + Consecutivo + " no existe");
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }
        #endregion
    }
}
