using System;
using System.Collections.Generic;
using System.Linq;

namespace Investigacion.Model.CustomEntities {
    /*
     * Clase custom para poder generar
     * resultados paginados y con informacion util.
     */
    public class Paginacion<T> : List<T> {

        #region Atributos
        public int TotalPaginas { get; set; } // Determina el total de paginas
        public int TamanoPaginas { get; set; } // Determina la cantidad de registros por pagina
        public int PaginaActual { get; set; } // Determina el numero de pagina actual
        public int CantidadRegistros { get; set; } // Cantidad de registros que se tienen

        // Atributos calculados
        public bool PaginaAnterior => PaginaActual > 1;
        public bool PaginaSiguiente => PaginaActual < TotalPaginas;
        public int? NumeroPaginaSiguiente => PaginaSiguiente ? PaginaActual + 1 : (int?)null;
        public int? NumeroPaginaAnterior => PaginaAnterior ? PaginaActual - 1 : (int?)null;
        #endregion

        #region Constructor
        private Paginacion(List<T> Elementos, int Cantidad, int PaginaActual, int TamanoPaginas) {
            this.CantidadRegistros = Cantidad;
            this.PaginaActual = PaginaActual;
            this.TamanoPaginas = TamanoPaginas;
            TotalPaginas = (int)Math.Ceiling(Cantidad / (double)TamanoPaginas);

            AddRange(Elementos);
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Genera los la informacion necesaria para la paginacion
        /// </summary>
        public static Paginacion<T> Paginar(IEnumerable<T> Raiz, int NumeroPagina, int TamanoPagina) {

            var Tamano = Raiz.Count();
            var Elementos = Raiz.Skip((NumeroPagina - 1) * TamanoPagina).Take(TamanoPagina).ToList();

            return new Paginacion<T>(Elementos, Tamano, NumeroPagina, TamanoPagina);
        }
        #endregion


    }
}
