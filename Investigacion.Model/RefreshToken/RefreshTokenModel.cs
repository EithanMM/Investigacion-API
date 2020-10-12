using Investigacion.Model.EntidadBase;
using System;

namespace Investigacion.Model {
    public class RefreshTokenModel : BaseModel {

        #region Constructor
        /// <summary>
        /// El constructor por defecto va a tener la fecha de creacion y expiracion seteada
        /// </summary>
        public RefreshTokenModel() {
            this.FechaCreacion = DateTime.Now;
            this.FechaExpiracion = DateTime.UtcNow.AddMonths(12);
            this.Revocado = null;
        }
        #endregion

        #region Atributos
        public System.Guid IdUsuario { get; set; }
        public string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool TokenExpirado => DateTime.UtcNow >= FechaExpiracion;
        public DateTime FechaCreacion { get; set; }
        public DateTime? Revocado { get; set; }
        public bool TokenActivo => Revocado == null && !TokenExpirado;

        public UsuarioModel UsuarioModel { get; set; }
        #endregion
    }
}
