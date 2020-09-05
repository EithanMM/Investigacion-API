using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_TRABAJO
    {
        public Guid LLP_Id { get; set; }
        public Guid LLF_Investigador { get; set; }
        public Guid LLF_TipoTrabajo { get; set; }
        public int NumeroRegistro { get; set; }
        public Guid? Consecutivo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        public virtual TAB_INVESTIGADOR LLF_InvestigadorNavigation { get; set; }
        public virtual TAB_TIPO_TRABAJO LLF_TipoTrabajoNavigation { get; set; }
    }
}
