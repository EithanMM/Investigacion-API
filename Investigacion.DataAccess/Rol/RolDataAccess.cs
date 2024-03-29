﻿using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Rol.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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

        public async Task<string> Actualizar(ActualizarRolDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.ActualizarRolUDT(ModeloDTO);
                var Nodo = new { Rol = Registro.AsTableValuedParameter("RolUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_ACTUALIZAR_ROL]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    RolModel RespuestaDTO = Mapper.Map<RolModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Obtener(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                RolModel Resultado = (await DbConexion.QueryAsync<RolModel>("dbo.[SP_OBTENER_ROL]", param: dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {
                IEnumerable<RolModel> Resultado = await DbConexion.QueryAsync<RolModel>("dbo.[SP_OBTENER_TODOS_ROL]", commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SaltoRegistros", RegistrosOmitidos);
                dynamicParameters.Add("TamanoPagina", TamanoPagina);
                dynamicParameters.Add("Total", null, DbType.Int32, direction: ParameterDirection.Output); //Nos permite leer el resultado.

                var Resultado = await DbConexion.QueryMultipleAsync("dbo.[SP_OBTENER_TODOS_ROL_PAGINACION]", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                EntidadPaginacion<RolModel> Respuesta = new EntidadPaginacion<RolModel>(await Resultado.ReadAsync<RolModel>(), dynamicParameters.Get<int>("@Total"));
                return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
            }
        }
        #endregion

    }
}
