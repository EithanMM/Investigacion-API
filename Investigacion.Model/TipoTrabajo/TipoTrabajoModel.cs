using Investigacion.Model.EntidadBase;

namespace Investigacion.Model {
    public class TipoTrabajoModel : BaseModel {

        #region Constructor
        public TipoTrabajoModel() { }

        public TipoTrabajoModel(string Error) {
            this.Error = Error;
        }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Descripcion { get; set; }
        #endregion
    }
}
