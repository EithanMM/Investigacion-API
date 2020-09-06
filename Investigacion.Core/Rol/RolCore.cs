using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Rol.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class RolCore : ILecturaCore<RolModel>,
                           IEscrituraCore<RolModel, AgregarRolDTO, ActualizarRolDTO> {

        #region Atrobutos y constructor
        private readonly ILecturaDataAccess<RolModel> RolLectura;
        private readonly IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> RolEscritura;
        private static int PosicionMensajeError = 6;
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        public RolCore(ILecturaDataAccess<RolModel> RolLectura, IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> RolEscritura) {
            this.RolLectura = RolLectura;
            this.RolEscritura = RolEscritura;
        }
        #endregion

        #region Metodos
        public Task<RolModel> Actualizar(ActualizarRolDTO Modelo) {
            throw new System.NotImplementedException();
        }

        public async Task<RolModel> Agregar(AgregarRolDTO Modelo) {

            RolModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await RolEscritura.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<RolModel>(Resultado);
            if (Respuesta.Error != null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public Task<IEnumerable<RolModel>> Listar() {
            throw new System.NotImplementedException();
        }

        public Task<Paginacion<RolModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {
            throw new System.NotImplementedException();
        }

        public Task<RolModel> Obtener(string Consecutivo) {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
