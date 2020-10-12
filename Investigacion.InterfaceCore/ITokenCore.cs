using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface ITokenCore<T, U> where T : class where U : class {

        /// <summary>
        /// Genera un token con la informacion del modelo pasado por parametro.
        /// </summary>
        string GenerarToken(T Modelo);

        /// <summary>
        /// Genera un token a partir de los claims.
        /// </summary>
        string GenerarToken(JwtSecurityToken Token);

        /// <summary>
        /// Obtiene los claims codificados dentro del token.
        /// </summary>
        JwtSecurityToken DecodificarToken(string Token);

        /// <summary>
        /// Extraemos la fecha del token brindando
        /// </summary>
        string ObtenerFechaToken(JwtSecurityToken Token);

        /// <summary>
        /// Metodo para comparar fecha y hora contra una fecha y hora actual
        /// </summary>
        bool CompararFechas(DateTime FechaComparacion);

        /// <summary>
        /// Genera un token de refrescamiento con la informacion del modelo pasado por parametro
        /// </summary>
        U GenerarRefreshToken(T Modelo);

        /// <summary>
        /// Agregamor el refresh token a la BD
        /// </summary>
        Task<U> AgregarRefreshToken(U Modelo);

        /// <summary>
        /// Obtenemos el refresh token segun el id 
        /// </summary>
        Task<U> Obtener(System.Guid Id);

        /// <summary>
        /// Obtenemos el refresh token segun nombre de usaurio y email 
        /// </summary>
        Task<U> Obtener(string Usuario, string Email);

        /// <summary>
        /// Generamos un nuevo token de acceso cuando el actual expire.
        /// </summary>
        Task<U> GenerarTokenAcceso(ClaimsIdentity Identidad);
    }
}
