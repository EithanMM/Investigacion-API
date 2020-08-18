using Investigacion.Model;
using Investigacion.Model.CustomEntities;

namespace Investigacion.WebApi.Helpers {
    public class PaginationHelper<T> {

        #region Metodos
        /// <summary>
        /// Obtiene un objeto que contiene informacion de paginacion.
        /// </summary>
        public static object SetXPaginationHeaderData(Paginacion<InvestigadorModel> Datos) {
            return new {
                Datos.PaginaActual,
                Datos.TotalPaginas,
                Datos.TamanoPaginas,
                Datos.CantidadRegistros
            };
        }

        /// <summary>
        /// Retorna un objeto de clase Metadata con la informacion de paginacion
        /// </summary>
        public static Metadata SetMetaData(Paginacion<T> Datos) {
            return new Metadata {
                TotalPaginas = Datos.TotalPaginas,
                TamanoPaginas = Datos.TamanoPaginas,
                PaginaActual = Datos.PaginaActual,
                CantidadRegistros = Datos.CantidadRegistros,
                NumeroPaginaAnterior = Datos.NumeroPaginaAnterior,
                NumeroPaginaSiguiente = Datos.NumeroPaginaSiguiente,
                PaginaAnterior = Datos.PaginaAnterior,
                PaginaSiguiente = Datos.PaginaSiguiente
            };
        }
        #endregion
    }
}
