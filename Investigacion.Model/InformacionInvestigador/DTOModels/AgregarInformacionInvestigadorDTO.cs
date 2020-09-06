using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.InformacionInvestigador.DTOModels {
    public class AgregarInformacionInvestigadorDTO {

        #region Atributos
        public string IdInvestigador { get; set; }
        public string IdEspecialidad { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }
        #endregion
    }
}
