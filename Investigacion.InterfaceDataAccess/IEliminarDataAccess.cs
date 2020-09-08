using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface IEliminarDataAccess {

        /// <summary>
        /// Elimina un registro de la BD segun un consecutivo
        /// </summary>
        Task<bool> Eliminar(string Consecutivo);
    }
}
