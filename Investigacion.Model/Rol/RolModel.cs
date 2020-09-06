using Investigacion.Model.EntidadBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model {
    public class RolModel : BaseModel {

        #region Constructor
        public RolModel() { }
        public RolModel(string Error) {
            this.Error = Error;
        }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Descripcion { get; set; }
        #endregion
    }
}
