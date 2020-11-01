using Investigacion.Model.EntidadBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model {
    public class EspecialidadModel : BaseModel {

        #region Constructor
        public EspecialidadModel() { }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Descripcion { get; set; }
        #endregion
    }
}
