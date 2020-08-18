using Investigacion.Model.Trabajo.DTOModels;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface TrabajoInterfaceDataAccess {

        /// <summary>
        /// Agrega un registro de Trabajo a la BD
        /// </summary>
        Task<string> Agregar(AgregarTrabajoDTO ModeloDTO);

        /// <summary>
        /// Obtiene un registro de Trabajo a la BD segun su consecutivo
        /// </summary>
        Task<string> Obtener(string Consecutivo);

        /// <summary>
        /// Obtiene todos los registros de Trabajo a la BD
        /// </summary>
        Task<string> Listar();

        /// <summary>
        /// Elimina un registro de Trabajo a la BD segun su consecutivo.
        /// </summary>
        Task<string> Eliminar(string Consecutivo);

    }
}
