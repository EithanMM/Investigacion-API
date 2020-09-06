using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Rol.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class RolDataAccess : ILecturaDataAccess<RolModel>,
                                 IEscrituraDataAccess<AgregarRolDTO, ActualizarRolDTO> {


        #region Variables y constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public RolDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion


        #region Metodos
        #endregion

        public async Task<string> Actualizar(ActualizarRolDTO ModeloDTO) {
            throw new System.NotImplementedException();
        }

        public async Task<string> Agregar(AgregarRolDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarRolUDT(ModeloDTO);
                var Nodo = new { Rol = Registro.AsTableValuedParameter("RolUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_ROL]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    RolModel RespuestaDTO = Mapper.Map<RolModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Listar() {
            throw new System.NotImplementedException();
        }

        public async Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {
            throw new System.NotImplementedException();
        }

        public async Task<string> Obtener(string Consecutivo) {
            throw new System.NotImplementedException();
        }
    }
}
