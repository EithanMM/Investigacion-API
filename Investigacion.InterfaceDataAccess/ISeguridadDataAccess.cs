using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface ISeguridadDataAccess<T, U> where T : class where U : class {

        /// <summary>
        /// Actualiza el password del usuario
        /// </summary>
        Task<bool> CambiarPassword(T Modelo);

        /// <summary>
        /// Verificar si el usuario existe
        /// </summary>
        Task<string> AutenticarUsuario(U Modelo);
    }
}
