using Investigacion.Model.EntidadBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model {
    public class InformacionInvestigadorModel : BaseModel {

        #region Constrcutor
        public InformacionInvestigadorModel() { }
        public InformacionInvestigadorModel(string Error) {
            this.Error = Error;
        }
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
