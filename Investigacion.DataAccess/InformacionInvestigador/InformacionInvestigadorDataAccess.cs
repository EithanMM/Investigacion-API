using AutoMapper;
using Dapper;
using Investigacion.DataAccess.Helper;
using Investigacion.InterfaceDataAccess;
using Investigacion.Model;
using Investigacion.Model.InformacionInvestigador.DTOModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investigacion.DataAccess {
    public class InformacionInvestigadorDataAccess : ILecturaDataAccess<InformacionInvestigadorModel>,
                                                     IEscrituraDataAccess<AgregarInformacionInvestigadorDTO, ActualizarInformacionInvestigadorDTO> {


        #region Variables y constrcutor
        private readonly IMapper Mapper;
        private string ConnectionString = "";
        private readonly IConfiguration Configuration;

        public InformacionInvestigadorDataAccess(IMapper Mapper, IConfiguration Configuration) {
            this.Mapper = Mapper;
            ConnectionString = Configuration.GetConnectionString("INVESTIGACION_DB");
        }
        #endregion
        #region Metodos
        public async Task<string> Agregar(AgregarInformacionInvestigadorDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.AgregarInformacionInvestigadorUDT(ModeloDTO);
                var Nodo = new { InformacionInvestigador = Registro.AsTableValuedParameter("InformacionInvestigadorUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_AGREGAR_INFORMACION_INVESTIGADOR]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    InformacionInvestigadorModel RespuestaDTO = Mapper.Map<InformacionInvestigadorModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Actualizar(ActualizarInformacionInvestigadorDTO ModeloDTO) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                DataTable Registro = DataSetHelper.ActualizarInformacionInvestigadorUDT(ModeloDTO);
                var Nodo = new { InformacionInvestigador = Registro.AsTableValuedParameter("InformacionInvestigadorUDT") };
                dynamic Resultado = (await DbConexion.QueryAsync("dbo.[SP_ACTUALIZAR_INFORMACION_INVESTIGADOR]", Nodo, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (!DataHelper.VerificarAtributo(Resultado, "Error")) {
                    InformacionInvestigadorModel RespuestaDTO = Mapper.Map<InformacionInvestigadorModel>(Resultado);
                    return Utf8Json.JsonSerializer.ToJsonString(RespuestaDTO);
                }
                else return "Error:" + Resultado.Error;
            }
        }

        public async Task<string> Listar() {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                IEnumerable<InformacionInvestigadorModel> Resultado = await DbConexion.QueryAsync<InformacionInvestigadorModel, InvestigadorModel, EspecialidadModel, InformacionInvestigadorModel>("dbo.[SP_OBTENER_TODOS_INFORMACION_INVESTIGADOR]",
                    map: (tii, ti, te) => {
                        tii.InvestigadorModel = ti;
                        tii.EspecialidadModel = te;
                        return tii;

                    }, splitOn: "IdInformacionInvestigador, IdInvestigador, IdEspecialidad",
                    commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }

        public async Task<string> Obtener(string Consecutivo) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Consecutivo", Consecutivo);
                InformacionInvestigadorModel Resultado = (await DbConexion.QueryAsync<InformacionInvestigadorModel>("dbo.[SP_OBTENER_INFORMACION_INVESTIGADOR]", param: dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                return Resultado != null ? Utf8Json.JsonSerializer.ToJsonString(Resultado) : "";
            }
        }

        public async Task<string> ListarPaginacion(int RegistrosOmitidos, int TamanoPagina) {

            using (IDbConnection DbConexion = new SqlConnection(ConnectionString)) {

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SaltoRegistros", RegistrosOmitidos);
                dynamicParameters.Add("TamanoPagina", TamanoPagina);
                dynamicParameters.Add("Total", null, DbType.Int32, direction: ParameterDirection.Output); //Nos permite leer el resultado.

                IEnumerable<InformacionInvestigadorModel> Resultado = await DbConexion.QueryAsync<InformacionInvestigadorModel, InvestigadorModel, EspecialidadModel, InformacionInvestigadorModel>("dbo.[SP_OBTENER_TODOS_INFORMACION_INVESTIGADOR_PAGINACION]",
                    param: dynamicParameters,
                    map: (tii, ti, te) => {
                        tii.InvestigadorModel = ti;
                        tii.EspecialidadModel = te;
                        return tii;

                    }, splitOn: "IdInformacionInvestigador, IdInvestigador, IdEspecialidad",
                    commandType: CommandType.StoredProcedure);
                return Utf8Json.JsonSerializer.ToJsonString(Resultado);
            }
        }
        #endregion
    }
}
