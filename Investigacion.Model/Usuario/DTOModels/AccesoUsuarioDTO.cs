namespace Investigacion.Model.Usuario.DTOModels {
    public class AccesoUsuarioDTO {

        #region Constructor
        public AccesoUsuarioDTO() {}
        public AccesoUsuarioDTO(string Usuario, string Password) {
            this.Usuario = Usuario;
            this.Password = Password;
        }
        #endregion
        #region Atributos
        public string Usuario { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
