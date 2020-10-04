using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Especialidad.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class EspecialidadDataAccess : ILecturaDataAccess<EspecialidadModel>,
                                          IEscrituraDataAccess<AgregarEspecialidadDTO, ActualizarEspecialidadDTO> {

        #region Variables y Constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public EspecialidadDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion

        #region Metodos
        public async Task<string> Agregar(AgregarEspecialidadDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarEspecialidadUDT(ModeloDTO);
                var Nodo = new { @Especialidad = Registro.AsTableValuedParameter("EspecialidadUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_ESPECIALIDAD]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    EspecialidadModel RespuestaDTO = Mapper.Map<EspecialidadModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Actualizar(ActualizarEspecialidadDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.ActualizarEspecialidadUDT(ModeloDTO);
                var Nodo = new { @Especialidad = Registro.AsTableValuedParameter("EspecialidadUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_ACTUALIZAR_ESPECIALIDAD]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    EspecialidadModel RespuestaDTO = Mapper.Map<EspecialidadModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Obtener(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                EspecialidadModel Resultado = (await DbConexion.QueryAsync<EspecialidadModel>("dbo.[SP_OBTENER_ESPECIALIDAD]", param: dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {
                IEnumerable<EspecialidadModel> Resultado = await DbConexion.QueryAsync<EspecialidadModel>("dbo.[SP_OBTENER_TODOS_ESPECIALIDAD]", commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SaltoRegistros", RegistrosOmitidos);
                dynamicParameters.Add("TamanoPagina", TamanoPagina);
                dynamicParameters.Add("Total", null, DbType.Int32, direction: ParameterDirection.Output); //Nos permite leer el resultado.

                var Resultado = await DbConexion.QueryMultipleAsync("dbo.[SP_OBTENER_TODOS_ESPECIALIDAD_PAGINACION]", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                EntidadPaginacion<EspecialidadModel> Respuesta = new EntidadPaginacion<EspecialidadModel>(await Resultado.ReadAsync<EspecialidadModel>(), dynamicParameters.Get<int>("@Total"));
                return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
            }
        }
        #endregion
    }
}
