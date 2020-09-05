using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_TIPO_TRABAJO
    {
        public TAB_TIPO_TRABAJO()
        {
            TAB_TRABAJO = new HashSet<TAB_TRABAJO>();
        }

        public Guid LLP_Id { get; set; }
        public int NumeroRegistro { get; set; }
        public Guid? Consecutivo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TAB_TRABAJO> TAB_TRABAJO { get; set; }
    }
}
