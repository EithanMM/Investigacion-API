using Investigacion.Model.TipoTrabajo.DTOModels;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface TipoTrabajoInterfaceDataAccess {

        /// <summary>
        /// Agrega un registro de tipo trabajo a la BD
        /// </summary>
        Task<string> Agregar(AgregarTipoTrabajoDTO ModeloDTO);

        /// <summary>
        /// obtiene un registro de tipo de trabajo a la BD segun su consecutivo
        /// </summary>
        Task<string> Obtener(string Consecutivo);

        /// <summary>
        /// Actualiza un registro de tipo de trabajo.
        /// </summary>
        Task<string> Actualizar(ActualizarTipoTrabajoDTO ModeloDTO);

        /// <summary>
        /// Lista todos los registros de tipos de trabajo
        /// </summary>
        Task<string> Listar();

        /// <summary>
        /// Elimina un registro de tipo de trabajo segun su consecutivo
        /// </summary>
        Task<bool> Eliminar(string Consecutivo);
    }
}
