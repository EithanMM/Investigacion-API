using System.Collections.Generic;
namespace Investigacion.Model {
    /*
         * Clase custom para la manipulacion
         * de datos paginados en la capa de DataAccess
         * al realizar consultas por SP.
     */
    public class EntidadPaginacion<T> where T : class {
        public EntidadPaginacion() { }
        public EntidadPaginacion(IEnumerable<T> Data, int Total) {
            this.Data = Data;
            this.Total = Total;
        }
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }

    }
}
