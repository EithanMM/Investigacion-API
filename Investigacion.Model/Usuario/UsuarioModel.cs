using Investigacion.Model.EntidadBase;
using System.Text.Json.Serialization;

namespace Investigacion.Model {
    public class UsuarioModel : BaseModel {

        #region Constructor
        public UsuarioModel() { }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public RolModel RolModel { get; set; }
        #endregion
    }
}
