using Investigacion.Model;
using Investigacion.Model.Trabajo.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface TrabajoInterfaceCore {

        /// <summary>
        /// Obtiene los registros de los trabajos.
        /// </summary>
        Task<TrabajoModel> Agregar(AgregarTrabajoDTO Modelo);

        /// <summary>
        /// Obtiene un registro de Trabajo a la BD segun su consecutivo
        /// </summary>
        Task<TrabajoModel> Obtener(string Consecutivo);

        /// <summary>
        /// Obtiene los registros de los trabajos.
        /// </summary>
        Task<IEnumerable<TrabajoModel>> Listar();

        /// <summary>
        /// Elimina un registro de Trabajo a la BD segun su consecutivo.
        /// </summary>
        Task<TrabajoModel> Eliminar(string Consecutivo);
    }
}
