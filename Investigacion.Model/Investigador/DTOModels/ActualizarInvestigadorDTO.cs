/********************************************
 MODELO PENSADO COMO CONTRATO PARA ENDPOINTS
 DE ACTUALIZAR UN INVESTIGADOR
 ********************************************/
namespace Investigacion.Model.Investigador.DTOModels {
    public class ActualizarInvestigadorDTO {

        #region Atributos
        public System.Guid Consecutivo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        #endregion
    }
}
