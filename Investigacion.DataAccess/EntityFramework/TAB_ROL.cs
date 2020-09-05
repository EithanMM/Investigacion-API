using System;
using System.Collections.Generic;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class TAB_ROL
    {
        public Guid LLP_Id { get; set; }
        public int NumeroRegistro { get; set; }
        public Guid? Consecutivo { get; set; }
        public string Descripcion { get; set; }
    }
}
