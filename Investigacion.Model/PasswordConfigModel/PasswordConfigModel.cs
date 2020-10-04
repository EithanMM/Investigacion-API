namespace Investigacion.Model {
    public class PasswordConfigModel {

        /* Atributos utilizados para la configuracion del password*/
        #region Atributos
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
        public int Iteraitons { get; set; }
        #endregion
    }
}
