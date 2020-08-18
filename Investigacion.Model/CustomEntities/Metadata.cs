namespace Investigacion.Model.CustomEntities {
    public class Metadata {

        #region Atributos
        public int TotalPaginas { get; set; } // Determina el total de paginas
        public int TamanoPaginas { get; set; } // Determina la cantidad de registros por pagina
        public int PaginaActual { get; set; } // Determina el numero de pagina actual
        public int CantidadRegistros { get; set; } // Cantidad de registros que se tienen
        public int? NumeroPaginaSiguiente { get; set; } //Guarda el numero de la pagina siguiente.
        public int? NumeroPaginaAnterior { get; set; } //Guarda el numero de la pagina anterior
        public bool PaginaAnterior { get; set; }
        public bool PaginaSiguiente { get; set; }

        //Atributos que guardan urls - HEITIUS 
        public string PaginaSiguienteUrl { get; set; }
        public string PaginaAnteriorUrl { get; set; }
        #endregion
    }
}
