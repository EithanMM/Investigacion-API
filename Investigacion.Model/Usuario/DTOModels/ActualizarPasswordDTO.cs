using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.Usuario.DTOModels {
    public class ActualizarPasswordDTO {

        #region Atributos
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
