namespace Investigacion.Model.Usuario.DTOModels {
    public class RespuestaUsuarioDTO {

        #region Constructor
        public RespuestaUsuarioDTO() { }
        public RespuestaUsuarioDTO(string Mensaje) {
            this.Mensaje = Mensaje;
        }
        #endregion

        #region Atributos
        public string Mensaje { get; set; }
        public string Token { get; set; }
        #endregion
    }
}
