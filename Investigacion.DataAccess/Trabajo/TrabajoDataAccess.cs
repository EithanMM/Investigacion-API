﻿using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.Trabajo.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class TrabajoDataAccess : ILecturaDataAccess<TrabajoModel>,
                                     IEscrituraDataAccess<AgregarTrabajoDTO, ActualizarTrabajoDTO> {

        #region Variables y constructor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public TrabajoDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion


        #region Metodos

        public async Task<string> Agregar(AgregarTrabajoDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarTrabajoUDT(ModeloDTO);
                var Nodo = new { Trabajos = Registro.AsTableValuedParameter("TrabajoUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_TRABAJO]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    TrabajoModel Respuesta = Mapper.Map<TrabajoModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(Respuesta);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public Task<string> Actualizar(ActualizarTrabajoDTO ModeloDTO) {
            throw new NotImplementedException();
        }

        public async Task<string> Obtener(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                TrabajoModel Resultado = (await DbConexion.QueryAsync<TrabajoModel>("dbo.[SP_OBTENER_TRABAJO]", param: dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                IEnumerable<RespuestaTrabajoDTO> Resultado = await DbConexion.QueryAsync<RespuestaTrabajoDTO, InvestigadorModel, TipoTrabajoModel, RespuestaTrabajoDTO>("dbo.[SP_OBTENER_TODOS_TRABAJO]",
                    map: (tt, ti, ttt) => {
                        tt.InvestigadorModel = ti;
                        tt.TipoTrabajoModel = ttt;
                        return tt;

                    }, splitOn: "IdTrabajo, IdInvestigador, IdTipoTrabajo",
                    commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
