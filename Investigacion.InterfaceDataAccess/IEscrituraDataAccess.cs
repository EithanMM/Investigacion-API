using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface IEscrituraDataAccess<T, U> where T : class where U : class{

        /// <summary>
        /// Metodo task para agregar un registro a la BD
        /// </summary>
        Task<string> Agregar(T ModeloDTO);

        /// <summary>
        /// Metodo task para actualizar un registro a la BD
        /// </summary>
        Task<string> Actualizar(U ModeloDTO);
    }
}
