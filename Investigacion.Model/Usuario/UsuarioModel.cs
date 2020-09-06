using Investigacion.Model.EntidadBase;

namespace Investigacion.Model {
    public class UsuarioModel : BaseModel {

        #region Constructor
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
