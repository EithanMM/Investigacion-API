using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.Especialidad.DTOModels {
    public class RespuestaEspecialidadDTO {

        #region Constructor
        public RespuestaEspecialidadDTO() { }
        #endregion

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Descripcion { get; set; }
        #endregion
    }
}
