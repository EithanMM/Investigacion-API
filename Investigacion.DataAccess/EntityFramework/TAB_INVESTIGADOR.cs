using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_INVESTIGADOR
    {
        public TAB_INVESTIGADOR()
        {
            TAB_INFORMACION_INVESTIGADOR = new HashSet<TAB_INFORMACION_INVESTIGADOR>();
            TAB_TRABAJO = new HashSet<TAB_TRABAJO>();
        }

        public Guid LLP_Id { get; set; }
        public Guid? Consecutivo { get; set; }
        public int NumeroRegistro { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public virtual ICollection<TAB_INFORMACION_INVESTIGADOR> TAB_INFORMACION_INVESTIGADOR { get; set; }
        public virtual ICollection<TAB_TRABAJO> TAB_TRABAJO { get; set; }
    }
}
