using System;

namespace Investigacion.Model.Trabajo.DTOModels {
    public class ActualizarTrabajoDTO {

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        #endregion
    }
}
