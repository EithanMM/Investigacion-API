﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.Usuario.DTOModels {
    public class ActualizarUsuarioDTO {

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        #endregion
    }
}
