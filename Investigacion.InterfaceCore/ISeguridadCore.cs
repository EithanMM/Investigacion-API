using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface ISeguridadCore<T> where T : class {

        /// <summary>
        /// Actualiza el password del usuario
        /// </summary>
        Task<bool> CambiarPassword(T Modelo);
    }
}
