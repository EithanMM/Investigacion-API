using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface ITokenDataAccess<T> where T : class {

        /// <summary>
        /// Registrar un refresh token a la BD
        /// </summary>
        Task<string> Agregar(T Modelo);

        /// <summary>
        /// Obtenemos el refresh token segun el id del usuario asociado
        /// </summary>
        Task<string> Obtener(System.Guid Identificador);

        /// <summary>
        /// Obtenemos el refresh token segun el usuario y su correo
        /// </summary>
        Task<string> Obtener(string Usuario, string Email);
    }
}
