using System;
using System.Text.Json.Serialization;

namespace Investigacion.Model.Trabajo.DTOModels {
    public class AgregarTrabajoDTO {

        #region Atributos
        [JsonIgnore]
        public System.Guid IdInvestigador { get; set; }
        [JsonIgnore]
        public System.Guid IdTipoTrabajo { get; set; }
        public string ConsecutivoInvestigador { get; set; }
        public string ConsecutivoTipoTrabajo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        #endregion
    }
}
