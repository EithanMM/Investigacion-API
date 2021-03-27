using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.InformacionInvestigador.DTOModels {
    public class RespuestaInformacionInvestigadorDTO {

        #region Constructor
        public RespuestaInformacionInvestigadorDTO() { }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public System.Guid IdInvestigador { get; set; }
        public System.Guid IdEspecialidad { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }
        public InvestigadorModel InvestigadorModel { get; set; }
        public EspecialidadModel EspecialidadModel { get; set; }
        #endregion
    }
}
