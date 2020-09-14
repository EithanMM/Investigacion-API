using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface ISeguridadCore<T,U,F> where T : class where U : class where F : class {

        /// <summary>
        /// Actualiza el password del usuario
        /// </summary>
        Task<bool> CambiarPassword(T Modelo);

        /// <summary>
        /// Verificar si el usuario existe en la BD.
        /// </summary>
        Task<F> AutenticarUsuario(U Modelo);
    }
}
