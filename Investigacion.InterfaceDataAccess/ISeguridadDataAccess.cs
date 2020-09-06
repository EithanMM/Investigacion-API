using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface ISeguridadDataAccess<T> where T : class {

        /// <summary>
        /// Actualiza el password del usuario
        /// </summary>
        Task<bool> CambiarPassword(T Modelo);
    }
}
