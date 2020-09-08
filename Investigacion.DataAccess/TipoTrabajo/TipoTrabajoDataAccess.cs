using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.TipoTrabajo.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class TipoTrabajoDataAccess : ILecturaDataAccess<TipoTrabajoModel>,
                                         IEscrituraDataAccess<AgregarTipoTrabajoDTO, ActualizarTipoTrabajoDTO>,
                                         IEliminarDataAccess {

        #region Variables y constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public TipoTrabajoDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion

        #region Metodos

        public async Task<string> Agregar(AgregarTipoTrabajoDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarTipoTrabajoUDT(ModeloDTO);
                var Nodo = new { TipoTrabajo = Registro.AsTableValuedParameter("TipoTrabajoUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_TIPO_TRABAJO]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    TipoTrabajoModel Respuesta = Mapper.Map<TipoTrabajoModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Actualizar(ActualizarTipoTrabajoDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.ActualizarTipoTrabajoUDT(ModeloDTO);
                var Nodo = new { TipoTrabajo = Registro.AsTableValuedParameter("TipoTrabajoUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_ACTUALIZAR_TIPO_TRABAJO]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    TipoTrabajoModel RespuestaDTO = Mapper.Map<TipoTrabajoModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {
                IEnumerable<TipoTrabajoModel> Resultado = await DbConexion.QueryAsync<TipoTrabajoModel>("dbo.[SP_OBTENER_TODOS_TIPO_TRABAJO]", commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> Obtener(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                TipoTrabajoModel Resultado = (await DbConexion.QueryAsync<TipoTrabajoModel>("dbo.[SP_OBTENER_TIPO_TRABAJO]", param: dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }

        public async Task<bool> Eliminar(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                int RespuestaBD = await DbConexion.ExecuteAsync("dbo.[SP_ELIMINAR_TIPO_TRABAJO]", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return (RespuestaBD > 0) ? true : false;
            }
        }

        public Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
