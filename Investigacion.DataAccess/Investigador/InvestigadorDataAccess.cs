﻿using AutoMapper;
using Dapper;
using Investigacion.DataAccess.EntityFramework;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Investigador.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Text.Json: usa JsonSerializer.Serialize(string);
using System.Threading.Tasks;

namespace Investigacion.DataAccess {

    public class InvestigadorDataAccess : ILecturaDataAccess<InvestigadorModel>,
                                          IEscrituraDataAccess<AgregarInvestigadorDTO, ActualizarInvestigadorDTO>,
                                          IEliminarDataAccess<InvestigadorModel> {

        #region Variables y Constructor
        private readonly InvestigacionDBContext Contexto;
        private readonly IMapper Mapper;
        private string ConnectionString = "";

        //Usando EF normal
        public InvestigadorDataAccess(InvestigacionDBContext Contexto, IMapper Mapper) {
            this.Contexto = Contexto;
            this.Mapper = Mapper;
            this.ConnectionString = Contexto.Database.GetDbConnection().ConnectionString;
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
                else return "Error:" + Resultado.Error;
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
                else return "Error:" + Resultado.Error;
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
        #endregion
    }
}