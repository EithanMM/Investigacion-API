using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Investigador.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Text.Json: usa JsonSerializer.Serialize(string);
using System.Threading.Tasks;

namespace Investigacion.DataAccess {

    public class InvestigadorDataAccess : ILecturaDataAccess<InvestigadorModel>,
                                          IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO>,
                                          IEliminarDataAccess {

        #region Variables y Constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public InvestigadorDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion

        #region Metodos
        public async Task<string> Agregar(AgregarInvestigadorDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarInvestigadorUDT(ModeloDTO);
                var Nodo = new { Investigadores = Registro.AsTableValuedParameter("InvestigadorUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_INVESTIGADOR]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    InvestigadorModel RespuestaDTO = Mapper.Map<InvestigadorModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else {
                    ErrorModel Error = Mapper.Map<ErrorModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(Error);
                }
            }
        }


        public async Task<string> Actualizar(ActualizarInvestigadorDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.ActualizarInvestigadorUDT(ModeloDTO);
                var Nodo = new { Investigadores = Registro.AsTableValuedParameter("InvestigadorUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_ACTUALIZAR_INVESTIGADOR]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    InvestigadorModel RespuestaDTO = Mapper.Map<InvestigadorModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else {
                    ErrorModel Error = Mapper.Map<ErrorModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(Error);
                }
            }
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {
                IEnumerable<InvestigadorModel> Resultado = await DbConexion.QueryAsync<InvestigadorModel>("dbo.[SP_OBTENER_TODOS_INVESTIGADOR]", commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> Obtener(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                InvestigadorModel Resultado = (await DbConexion.QueryAsync<InvestigadorModel>("dbo.[SP_OBTENER_INVESTIGADOR]", param: dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }

        public async Task<bool> Eliminar(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                int RespuestaBD = await DbConexion.ExecuteAsync("dbo.[SP_ELIMINAR_INVESTIGADOR]", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return (RespuestaBD > 0) ? true : false;
            }
        }

        public async Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SaltoRegistros", RegistrosOmitidos);
                dynamicParameters.Add("TamanoPagina", TamanoPagina);
                dynamicParameters.Add("Total", null, DbType.Int32, direction: ParameterDirection.Output); //Nos permite leer el resultado.

                var Resultado = await DbConexion.QueryMultipleAsync("dbo.[SP_OBTENER_TODOS_INVESTIGADOR_PAGINACION]", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                EntidadPaginacion<InvestigadorModel> Respuesta = new EntidadPaginacion<InvestigadorModel>(await Resultado.ReadAsync<InvestigadorModel>(), dynamicParameters.Get<int>("@Total"));
                return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
            }
        }
        #endregion
    }
}
