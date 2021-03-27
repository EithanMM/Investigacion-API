using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model {
    public class ErrorModel {

        #region Constructor
        public ErrorModel() {}

        public ErrorModel(string Error) {
            this.Error = Error;
        }
        #endregion

        #region Atributos
        public string Error { get; set; }
        #endregion
    }
}
