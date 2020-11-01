using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.Rol.DTOModels {
    public class RespuestaRolDTO {

        #region Constructor
        public RespuestaRolDTO(string Error) {
            this.Error = Error;
        }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Descripcion { get; set; }
        public string Error { get; set; }
        #endregion
    }
}
