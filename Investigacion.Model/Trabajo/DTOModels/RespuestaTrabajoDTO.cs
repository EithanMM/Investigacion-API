using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.Trabajo.DTOModels {
    public class RespuestaTrabajoDTO {

        #region Constructor
        public RespuestaTrabajoDTO() {}
        #endregion

        #region Atributos
        public System.Guid IdInvestigador { get; set; }
        public System.Guid IdTipoTrabajo { get; set; }
        public string Consecutivo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        public InvestigadorModel InvestigadorModel { get; set; }
        public TipoTrabajoModel TipoTrabajoModel { get; set; }
        #endregion

    }
}
