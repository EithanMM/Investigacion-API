using Investigacion.Model.CustomEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface ILecturaCore<T> where T : class {

        /// <summary>
        /// Lista el total de elementos de la BD
        /// </summary>
        Task<IEnumerable<T>> Listar();

        /// <summary>
        /// Obtiene un regisro especifico segun su consecutivo
        /// </summary>
        Task<T> Obtener(string Consecutivo);

        /// <summary>
        /// Obtiene la lista de elementos segun su paginacions
        /// </summary>
        Task<Paginacion<T>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina);
    }
}
