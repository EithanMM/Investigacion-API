using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.CustomEntities {
    public class HateoasModel {

        #region Constructor
        public HateoasModel(string Href, string Rel, string Metodo) {
            this.Href = Href;
            this.Rel = Rel;
            this.Metodo = Metodo;
        }
        #endregion

        #region Atributos
        public string Href { get; private set; } //<- Url que se desea invocar
        public string Rel { get; private set; } //<- Determina el tipo de accion que se desea realizar
        public string Metodo { get; private set; } // <- Determina el metodo que deseamos utilizar
        #endregion
    }
}
