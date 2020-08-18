using System;
using System.Text.Json.Serialization;

namespace Investigacion.Model.Trabajo.DTOModels {
    public class AgregarTrabajoDTO {

        #region Atributos
        [JsonIgnore] //< - Omite que se presente este atributo en el swagger.
        public int IdInvestigador { get; set; }
        [JsonIgnore]
        public int IdTipoTrabajo { get; set; }
        public string ConsecutivoInvestigador { get; set; }
        public string ConsecutivoTipoTrabajo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        #endregion
    }
}
