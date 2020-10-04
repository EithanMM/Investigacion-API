using System;
using System.Collections.Generic;
using System.Text;

namespace Investigacion.Model.RefreshToken {
    public class RefreshTokenModel {
        public string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool TokenExpirado => DateTime.UtcNow >= FechaExpiracion;
        public DateTime FechaCreacion { get; set; }
        public DateTime Revocado { get; set; }
        public bool TokenActivo => Revocado == null && !TokenExpirado;
    }
}
