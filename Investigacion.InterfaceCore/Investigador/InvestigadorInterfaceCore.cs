using Investigacion.Model;
using Investigacion.Model.CustomEntities;
using Investigacion.Model.Investigador.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investigacion.InterfaceCore {
    public interface InvestigadorInterfaceCore {

        /// <summary>
        /// Agregar un registro de Investigador a la BD
        /// </summary>
        Task<InvestigadorModel> Agregar(AgregarInvestigadorDTO Modelo);

        /// <summary>
        /// Actualiza un registro de investigador a la BD
        /// </summary>
        Task<InvestigadorModel> Actualizar(ActualizarInvestigadorDTO Modelo);

        /// <summary>
        /// Obtiene un registro de investigador a la BD segun su consecutivo
        /// </summary>
        Task<InvestigadorModel> Obtener(string Consecutivo);

        /// <summary>
        /// Obtiene todos los registros de investigador
        /// </summary>
        Task<IEnumerable<InvestigadorModel>> Listar();

        /// <summary>
        /// Obtiene todos los registros de investigador segun su paginacion
        /// </summary>
        Task<Paginacion<InvestigadorModel>> ListarPaginacion(int? NumeroPagina, int? TamanoPagina);

        /// <summary>
        /// Elimina un registro de investigador a la BD
        /// </summary>
        Task<bool> Eliminar(string Consecutivo);
    }
}
