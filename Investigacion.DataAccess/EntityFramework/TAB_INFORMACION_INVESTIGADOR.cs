using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_INFORMACION_INVESTIGADOR
    {
        public int LLP_Id { get; set; }
        public int LLF_Investigador { get; set; }
        public int LLF_Especialidad { get; set; }
        public string Consecutivo { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }

        public virtual TAB_ESPECIALIDAD LLF_EspecialidadNavigation { get; set; }
        public virtual TAB_INVESTIGADOR LLF_InvestigadorNavigation { get; set; }
    }
}
