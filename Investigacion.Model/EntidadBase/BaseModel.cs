using System.Text.Json.Serialization;

namespace Investigacion.Model.EntidadBase {
    public class BaseModel {

        #region Atributos
        //Atributos que todas las entidades poseen sin excepcion alguna.
        [JsonIgnore] /* Provoca que se ignore este atributo en swagger */
        public int Id { get; set; }
        public string Error { get; set; }
        #endregion
    }
}
