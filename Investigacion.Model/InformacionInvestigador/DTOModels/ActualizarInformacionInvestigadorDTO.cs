using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.InformacionInvestigador.DTOModels {
    public class ActualizarInformacionInvestigadorDTO {

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }
        #endregion
    }
}
