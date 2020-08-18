using System.Collections.Generic;

namespace Investigacion.DataAccess.Helper {
    public class DataHelper {

        #region Metodos

        /// <summary>
        /// Verifica si el objeto de tipo Dynamic posee un atributo con el Nombre introducido.
        /// </summary>
        public static bool VerificarAtributo(dynamic Objeto, string Nombre) {

            if (Objeto is IDictionary<string, object> Diccionario) {
                bool r = Diccionario.ContainsKey(Nombre);
                return r;
            }
            else return false;
        }
        #endregion
    }
}
