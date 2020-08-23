using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface IEliminarCore<T> where T : class {

        /// <summary>
        /// Elimina un registro de la BD segun un consecutivo
        /// </summary>
        Task<bool> Eliminar(string Consecutivo);
    }
}
