using Investigacion.Core.Excepciones;
using Investigacion.InterfaceCore;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Usuario.DTOModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.Core {
    public class UsuarioCore : ILecturaCore<UsuarioModel>,
                               IEscrituraCore<UsuarioModel, AgregarUsuarioDTO, ActualizarUsuarioDTO>,
                               ISeguridadCore<ActualizarPasswordDTO> {

        #region Variables y constructor
        private static int PaginaDefault = 1;
        private static int RegistrosDefault = 5;
        private static int PosicionMensajeError = 6;
        private readonly ILecturaDataAccess<UsuarioModel> ILecturaUsuario;
        private readonly ISeguridadDataAccess<ActualizarPasswordDTO> ISeguridadUsuario;
        private readonly IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO> IEscrituraUsuario;

        public UsuarioCore(ILecturaDataAccess<UsuarioModel> UsuarioLectura, ISeguridadDataAccess<ActualizarPasswordDTO> UsuarioSeguridad, IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO> UsuarioEscritura) {
            this.ILecturaUsuario = UsuarioLectura;
            this.ISeguridadUsuario = UsuarioSeguridad;
            this.IEscrituraUsuario = UsuarioEscritura;
        }
        #endregion

        #region Metodos
        public async Task<UsuarioModel> Agregar(AgregarUsuarioDTO Modelo) {

            UsuarioModel Respuesta;

            if (Modelo == null) throw new ExcepcionCore("Modelo nulo");
            string Resultado = await IEscrituraUsuario.Agregar(Modelo);
            Respuesta = Utf8Json.JsonSerializer.Deserialize<UsuarioModel>(Resultado);
            if (Respuesta == null) throw new ExcepcionCore(Resultado.Substring(PosicionMensajeError));
            return Respuesta;
        }

        public Task<UsuarioModel> Actualizar(ActualizarUsuarioDTO Modelo) {
            throw new NotImplementedException();
        }

        public async Task<UsuarioModel> Obtener(string Consecutivo) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioModel>> Listar() {

            var Respuesta = await ILecturaUsuario.Listar();
            if (Respuesta == null) throw new ExcepcionCore("Modelo nulo");
            IEnumerable<UsuarioModel> Resultado = Utf8Json.JsonSerializer.Deserialize<IEnumerable<UsuarioModel>>(Respuesta);
            return Resultado;
        }

        public async Task<Paginacion<UsuarioModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina) {

            NumeroPagina = (NumeroPagina > 0) ? NumeroPagina : PaginaDefault; 
            TamanoPagina = (TamanoPagina > 0) ? TamanoPagina : RegistrosDefault; 
            NumeroPagina = (NumeroPagina - 1);

            var Respuesta = await ILecturaUsuario.ListarPaginacion((int)NumeroPagina * (int)TamanoPagina, (int)TamanoPagina);
            EntidadPaginacion<UsuarioModel> Objeto = Utf8Json.JsonSerializer.Deserialize<EntidadPaginacion<UsuarioModel>>(Respuesta);
            var RespuestaPaginada = Paginacion<UsuarioModel>.PaginarSQL(Objeto.Data, (int)NumeroPagina, (int)TamanoPagina, Objeto.Total);
            return RespuestaPaginada;
        }

        public async Task<bool> CambiarPassword(ActualizarPasswordDTO Modelo) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
