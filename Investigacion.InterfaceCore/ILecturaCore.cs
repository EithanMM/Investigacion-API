using Investigacion.Model.CustomEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface ILecturaCore<T> where T : class {

        Task<IEnumerable<T>> Listar();

        Task<T> Obtener(string Consecutivo);

        Task<Paginacion<T>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina);
    }
}
