using Investigacion.Model.EntidadBase;

namespace Investigacion.Model {
    public class InvestigadorModel : BaseModel {

        #region Constructor
        public InvestigadorModel() { }
        public InvestigadorModel(string Error) {
            this.Error = Error;
        }
        #endregion

        #region Atributos
        public string Consecutivo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        #endregion
    }
}
