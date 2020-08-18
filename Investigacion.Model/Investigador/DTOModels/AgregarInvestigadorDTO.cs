/********************************************
 MODELO PENSADO COMO CONTRATO PARA ENDPOINTS
 DE AGREGAR UN INVESTIGADOR
 ********************************************/
namespace Investigacion.Model.Investigador.DTOModels {
    public class AgregarInvestigadorDTO {

        #region Atributos
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        #endregion

    }
}
