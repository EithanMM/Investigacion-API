﻿using Investigacion.Model.CustomEntities;
using Investigacion.Model.EntidadBase;
using System.Collections.Generic;

namespace Investigacion.Model {
    public class InvestigadorModel : BaseModel {

        #region Constructor
        public InvestigadorModel() { }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        #endregion
    }
}
