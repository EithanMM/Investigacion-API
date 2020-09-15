namespace Investigacion.Model {
    public class PasswordConfigModel {

        #region Atributos
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
        public int Iteraitons { get; set; }
        #endregion
    }
}
