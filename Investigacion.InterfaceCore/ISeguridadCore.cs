using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface ISeguridadCore<T,U,F> where T : class where U : class where F : class {

        /// <summary>
        /// Actualiza el password del usuario
        /// </summary>
        Task<bool> CambiarPassword(T Modelo);

        /// <summary>
        /// Meotod para cifrar el password del usuario.
        /// </summary>
        string HashPassword(string Password);

        /// <summary>
        /// Metodo para verificar que el password brindado corresponde al usuario.
        /// </summary>
        bool VerificarPassword(string Hash, string Password);

        /// <summary>
        /// Verificar si el usuario existe en la BD.
        /// </summary>
        Task<F> AutenticarUsuario(U Modelo);
    }
}
