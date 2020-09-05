using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface ILecturaDataAccess<T> where T : class {

        /// <summary>
        /// Obtiene todos los registros de la BD
        /// </summary>
        Task<string> Listar();

        /// <summary>
        /// Obtiene un registro de la BD segun su consecutivo
        /// </summary>
        Task<string> Obtener(string Consecutivo);

        /// <summary>
        /// Obtiene los registros de la BD segun su paginacion especifica.
        /// </summary>
        Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina);
    }
}
