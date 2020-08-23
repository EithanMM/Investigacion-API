using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface IEscrituraCore<T,U,K> where T: class where U: class where K : class {

        /// <summary>
        /// Agrega un registro del Modelo brindado como parametro a la BD
        /// </summary>
        Task<T> Agregar(U Modelo);

        /// <summary>
        /// Actualiza un registro del Modelo brindado como parametro a la BD
        /// </summary>
        Task<T> Actualizar(K Modelo);
    }
}
