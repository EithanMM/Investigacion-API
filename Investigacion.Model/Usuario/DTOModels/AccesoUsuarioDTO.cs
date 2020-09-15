namespace Investigacion.Model.Usuario.DTOModels {
    public class AccesoUsuarioDTO {

        #region Constructor
        public AccesoUsuarioDTO() {}
        public AccesoUsuarioDTO(string Usuario) {
            this.Usuario = Usuario;
        }
        #endregion
        #region Atributos
        public string Usuario { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
