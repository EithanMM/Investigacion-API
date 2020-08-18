using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_ESPECIALIDAD
    {
        public TAB_ESPECIALIDAD()
        {
            TAB_INFORMACION_INVESTIGADOR = new HashSet<TAB_INFORMACION_INVESTIGADOR>();
        }

        public int LLP_Id { get; set; }
        public string Consecutivo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TAB_INFORMACION_INVESTIGADOR> TAB_INFORMACION_INVESTIGADOR { get; set; }
    }
}
