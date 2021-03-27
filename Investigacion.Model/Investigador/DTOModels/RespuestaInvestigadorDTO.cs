using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.Investigador.DTOModels {
    public class RespuestaInvestigadorDTO {

        #region Constructor
        public RespuestaInvestigadorDTO() { }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        #endregion
    }
}
