using System.Text.Json.Serialization;

namespace Investigacion.Model.EntidadBase {
    public class BaseModel {

        #region Atributos
        [JsonIgnore] /* Provoca que se ignore este atributo en swagger */
        public System.Guid Id { get; set; }
        public string Error { get; set; }
        #endregion
    }
}
