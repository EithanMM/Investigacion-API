using Investigacion.Model.EntidadBase;
using System;

namespace Investigacion.Model {
    public class TrabajoModel : BaseModel {

        #region Constructor
        public TrabajoModel() {
        }
        #endregion

        #region Atributos
        public int IdInvestigador { get; set; }
        public int IdTipoTrabajo { get; set; }
        public string Consecutivo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        public InvestigadorModel InvestigadorModel { get; set; }
        public TipoTrabajoModel TipoTrabajoModel { get; set; }
        #endregion
    }
}
