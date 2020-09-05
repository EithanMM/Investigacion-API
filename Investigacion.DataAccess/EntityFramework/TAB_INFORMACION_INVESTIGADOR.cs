using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_INFORMACION_INVESTIGADOR
    {
        public Guid LLP_Id { get; set; }
        public Guid LLF_Investigador { get; set; }
        public Guid LLF_Especialidad { get; set; }
        public int NumeroRegistro { get; set; }
        public Guid? Consecutivo { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }

        public virtual TAB_ESPECIALIDAD LLF_EspecialidadNavigation { get; set; }
        public virtual TAB_INVESTIGADOR LLF_InvestigadorNavigation { get; set; }
    }
}
