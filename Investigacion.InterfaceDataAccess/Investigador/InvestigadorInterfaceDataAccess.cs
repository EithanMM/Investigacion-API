using Investigacion.Model.Investigador.DTOModels;
using System.Threading.Tasks;

namespace Investigacion.InterfaceDataAccess {
    public interface InvestigadorInterfaceDataAccess {
        /// <summary>
        /// Obtenemos todos los registros de investigador
        /// </summary>
        Task<string> Listar();

        /// <summary>
        /// Agregar un registro de investigador a la BD
        /// </summary>
        Task<string> Agregar(AgregarInvestigadorDTO ModeloDTO);

        /// <summary>
        /// Actualiza un registro de investigador a la BD
        /// </summary>
        Task<string> Actualizar(ActualizarInvestigadorDTO ModeloDTO);

        /// <summary>
        /// Obtiene un registro de investigador a la BD segun su consecutivo
        /// </summary>
        Task<string> Obtener(string Consecutivo);

        /// <summary>
        /// Elimina un registro de investigador a la BD
        /// </summary>
        Task<bool> Eliminar(string Consecutivo);

    }
}
