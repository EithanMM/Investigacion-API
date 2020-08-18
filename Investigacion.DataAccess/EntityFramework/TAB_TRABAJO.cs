using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_TRABAJO
    {
        public int LLP_Id { get; set; }
        public int LLF_Investigador { get; set; }
        public int LLF_TipoTrabajo { get; set; }
        public string Consecutivo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        public virtual TAB_INVESTIGADOR LLF_InvestigadorNavigation { get; set; }
        public virtual TAB_TIPO_TRABAJO LLF_TipoTrabajoNavigation { get; set; }
    }
}
