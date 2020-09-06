using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Usuario.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class UsuarioDataAccess : ILecturaDataAccess<UsuarioModel>,
                                     IEscrituraDataAccess<AgregarUsuarioDTO, ActualizarUsuarioDTO>,
                                     ISeguridadDataAccess<ActualizarPasswordDTO>{

        #region Variables y constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public UsuarioDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion

        #region Metodos
        public async Task<string> Agregar(AgregarUsuarioDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarUsuarioUDT(ModeloDTO);
                var Nodo = new { Usuario = Registro.AsTableValuedParameter("UsuarioUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_USUARIO]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    UsuarioModel RespuestaDTO = Mapper.Map<UsuarioModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Obtener(string Consecutivo) {
            throw new System.NotImplementedException();
        }

        public async Task<string> Actualizar(ActualizarUsuarioDTO ModeloDTO) {
            throw new System.NotImplementedException();
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                IEnumerable<UsuarioModel> Resultado = await DbConexion.QueryAsync<UsuarioModel>("dbo.[SP_OBTENER_TODOS_USUARIO]", commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SaltoRegistros", RegistrosOmitidos);
                dynamicParameters.Add("TamanoPagina", TamanoPagina);
                dynamicParameters.Add("Total", null, DbType.Int32, direction: ParameterDirection.Output); //Nos permite leer el resultado.

                var Resultado = await DbConexion.QueryMultipleAsync("dbo.[SP_OBTENER_TODOS_USUARIO_PAGINACION]", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                EntidadPaginacion<UsuarioModel> Respuesta = new EntidadPaginacion<UsuarioModel>(await Resultado.ReadAsync<UsuarioModel>(), dynamicParameters.Get<int>("@Total"));
                return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
            }
        }

        public async Task<bool> CambiarPassword(ActualizarPasswordDTO Modelo) {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
