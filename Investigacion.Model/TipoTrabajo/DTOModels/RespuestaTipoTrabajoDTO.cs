using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.TipoTrabajo.DTOModels {
    public class RespuestaTipoTrabajoDTO {

        #region Constructor
        public RespuestaTipoTrabajoDTO() {}
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Descripcion { get; set; }
        #endregion
    }
}
