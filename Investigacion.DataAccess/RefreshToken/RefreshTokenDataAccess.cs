using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class RefreshTokenDataAccess : ITokenDataAccess<RefreshTokenModel> {

        #region Variables y constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;
        public RefreshTokenDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion

        #region Metodos

        public async Task<string> Agregar(RefreshTokenModel Modelo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarRefreshTokenUDT(Modelo);
                var Nodo = new { Token = Registro.AsTableValuedParameter("RefreshTokenUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_REFRESH_TOKEN]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    RefreshTokenModel Respuesta = Mapper.Map<RefreshTokenModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Obtener(Guid Identificador) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("IdUsuario", Identificador);

                RefreshTokenModel Resultado = (await DbConexion.QueryAsync<RefreshTokenModel, UsuarioModel, RefreshTokenModel>("dbo.[SP_OBTENER_REFRESH_TOKEN]",
                    map: (rtm, um) => {
                        rtm.UsuarioModel = um;
                        return rtm;
                    }, splitOn: "IdRefreshToken, IdUsuario", param: dynamicParameters,
                       commandType: CommandType.StoredProcedure)).FirstOrDefault();

                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }

        public async Task<string> Obtener(string Usuario, string Email) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Usuario", Usuario);
                dynamicParameters.Add("Email", Email);

                RefreshTokenModel Resultado = (await DbConexion.QueryAsync<RefreshTokenModel, UsuarioModel, RefreshTokenModel>("dbo.[SP_OBTENER_REFRESH_TOKEN_USUARIO_EMAIL]",
                   map:(rtm, um) => {
                       rtm.UsuarioModel = um;
                       return rtm;
                   }, splitOn: "IdRefreshToken, IdUsuario", param: dynamicParameters,
                   commandType: CommandType.StoredProcedure)).FirstOrDefault();

                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }
        #endregion
    }
}
