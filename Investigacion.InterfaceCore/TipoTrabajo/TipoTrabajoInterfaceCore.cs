using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.TipoTrabajo.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface TipoTrabajoInterfaceCore {

        /// <summary>
        /// Agrega un registro de tipo de trabajo a la BD
        /// </summary>
        Task<TipoTrabajoModel> Agregar(AgregarTipoTrabajoDTO Modelo);

        /// <summary>
        /// obtiene un registro de tipo de trabajo a la BD segun su consecutivo
        /// </summary>
        Task<TipoTrabajoModel> Obtener(string Consecutivo);

        /// <summary>
        /// Actualiza un registro de tipo trabajo
        /// </summary>
        Task<TipoTrabajoModel> Actualizar(ActualizarTipoTrabajoDTO Modelo);

        /// <summary>
        /// Obtiene todos los registros de tipo trabajo
        /// </summary>
        Task<IEnumerable<TipoTrabajoModel>> Listar();

        /// <summary>
        /// Obtiene todos los registros de investigador segun su paginacion
        /// </summary>
        Task<Paginacion<TipoTrabajoModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina);

        /// <summary>
        /// Elimina un registro de tipo trabajo a la BD
        /// </summary>
        Task<bool> Eliminar(string Consecutivo);
    }
}
