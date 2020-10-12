using Investigacion.Model;
using Investigacion.Model.Especialidad.DTOModels;
using Investigacion.Model.InformacionInvestigador.DTOModels;
using Investigacion.Model.Investigador.DTOModels;
using Investigacion.Model.Rol.DTOModels;
using Investigacion.Model.TipoTrabajo.DTOModels;
using Investigacion.Model.Trabajo.DTOModels;
using Investigacion.Model.Usuario.DTOModels;
using System;
using System.Data;

namespace Investigacion.DataAccess.Helper {
    public class DataSetHelper {

        #region Metodos Agregar UDT

        /// <summary>
        /// Metodo Helper para ingresar un registro de Investigador
        /// </summary>
        public static DataTable AgregarInvestigadorUDT(AgregarInvestigadorDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Cedula", typeof(string));
            Resultado.Columns.Add("Nombre", typeof(string));
            Resultado.Columns.Add("Apellido", typeof(string));

            Resultado.Rows.Add(null, Modelo.Cedula, Modelo.Nombre, Modelo.Apellido);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para ingresar un registro de Trabajo
        /// </summary>
        public static DataTable AgregarTrabajoUDT(AgregarTrabajoDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("LLF_Investigador", typeof(System.Guid));
            Resultado.Columns.Add("LLF_TipoTrabajo", typeof(System.Guid));
            Resultado.Columns.Add("Nombre", typeof(string));
            Resultado.Columns.Add("Fecha", typeof(DateTime));

            Resultado.Rows.Add(null, Modelo.IdInvestigador, Modelo.IdTipoTrabajo, Modelo.Nombre, Modelo.Fecha);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para ingresar un registro de tipo trabajo
        /// </summary>
        public static DataTable AgregarTipoTrabajoUDT(AgregarTipoTrabajoDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Descripcion", typeof(string));

            Resultado.Rows.Add(null, Modelo.Descripcion);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para ingresar un registro de usuario
        /// </summary>
        public static DataTable AgregarUsuarioUDT(AgregarUsuarioDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Usuario", typeof(string));
            Resultado.Columns.Add("Email", typeof(string));
            Resultado.Columns.Add("Password", typeof(string));

            Resultado.Rows.Add(null, Modelo.Usuario, Modelo.Email, Modelo.Password);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para ingresar un registro de rol
        /// </summary>
        public static DataTable AgregarRolUDT(AgregarRolDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Descripcion", typeof(string));

            Resultado.Rows.Add(null, Modelo.Descripcion);
            return Resultado;

        }

        /// <summary>
        /// Metodo Helper para ingresar un registro de especialidad
        /// </summary>
        public static DataTable AgregarEspecialidadUDT(AgregarEspecialidadDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Descripcion", typeof(string));

            Resultado.Rows.Add(null, Modelo.Descripcion);
            return Resultado;
        }

        /// <summary>
        ///  Metodo Helper para ingresar un registro de informacion investigador
        /// </summary>
        public static DataTable AgregarInformacionInvestigadorUDT(AgregarInformacionInvestigadorDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("LLF_IdInvestigador", typeof(System.Guid));
            Resultado.Columns.Add("LLF_IdEsepcialidad", typeof(System.Guid));
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Email", typeof(string));
            Resultado.Columns.Add("Direccion", typeof(string));
            Resultado.Columns.Add("Pais", typeof(string));

            Resultado.Rows.Add(Modelo.IdInvestigador, Modelo.IdEspecialidad, null, Modelo.Email, Modelo.Direccion, Modelo.Pais);
            return Resultado;
        }

        /// <summary>
        ///  Metodo Helper para ingresar un registro de refresh token
        /// </summary>
        public static DataTable AgregarRefreshTokenUDT(RefreshTokenModel Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("LLP_Id", typeof(System.Guid));
            Resultado.Columns.Add("LLF_Usuario", typeof(System.Guid));
            Resultado.Columns.Add("NumeroRegistro", typeof(int));
            Resultado.Columns.Add("FechaExpiracion", typeof(DateTime));
            Resultado.Columns.Add("TokenExpirado", typeof(bool));
            Resultado.Columns.Add("FechaCreacion", typeof(DateTime));
            Resultado.Columns.Add("Revocado", typeof(DateTime));
            Resultado.Columns.Add("TokenActivo", typeof(bool));
            Resultado.Columns.Add("Token", typeof(string));

            Resultado.Rows.Add(
                                null,
                                Modelo.IdUsuario,
                                null,
                                Modelo.FechaExpiracion,
                                Modelo.TokenExpirado,
                                Modelo.FechaCreacion,
                                null,
                                Modelo.TokenActivo,
                                Modelo.Token);
            return Resultado;
        }
        #endregion

        #region Metodos Actualizar UDT

        /// <summary>
        /// Metodo Helper para actualizar un registro de Investigador
        /// </summary>
        public static DataTable ActualizarInvestigadorUDT(ActualizarInvestigadorDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Cedula", typeof(string));
            Resultado.Columns.Add("Nombre", typeof(string));
            Resultado.Columns.Add("Apellido", typeof(string));

            Resultado.Rows.Add(Modelo.Consecutivo, Modelo.Cedula, Modelo.Nombre, Modelo.Apellido);
            return Resultado;

        }

        /// <summary>
        /// Metodo Helper para actualizar un registro de Trabajo
        /// </summary>
        public static DataTable ActualizarTrabajoUDT(ActualizarTrabajoDTO Modelo) {
            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Nombre", typeof(string));
            Resultado.Columns.Add("Fecha", typeof(DateTime));

            Resultado.Rows.Add(Modelo.Consecutivo, Modelo.Nombre, Modelo.Fecha);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para actualizar un registro de tipo trabajo
        /// </summary>
        public static DataTable ActualizarTipoTrabajoUDT(ActualizarTipoTrabajoDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Descripcion", typeof(string));

            Resultado.Rows.Add(Modelo.Consecutivo, Modelo.Descripcion);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para actualizar un registro de rol
        /// </summary>
        public static DataTable ActualizarRolUDT(ActualizarRolDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Descripcion", typeof(string));

            Resultado.Rows.Add(Modelo.Consecutivo, Modelo.Descripcion);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para actualizar un registro de usuario
        /// </summary>
        public static DataTable ActualizarUsuarioUDT(ActualizarUsuarioDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Usuario", typeof(string));
            Resultado.Columns.Add("Email", typeof(string));

            Resultado.Rows.Add(Modelo.Consecutivo, Modelo.Usuario, Modelo.Email);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para actualizar el password de un registro de usuario
        /// </summary>
        public static DataTable ActualizarPasswordUsuarioUDT(ActualizarPasswordDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Usuario", typeof(string));
            Resultado.Columns.Add("Email", typeof(string));
            Resultado.Columns.Add("Password", typeof(string));

            Resultado.Rows.Add(null, Modelo.Usuario, Modelo.Email, Modelo.Password);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para actualizar un registro de especialidad
        /// </summary>
        public static DataTable ActualizarEspecialidadUDT(ActualizarEspecialidadDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Descripcion", typeof(string));

            Resultado.Rows.Add(Modelo.Consecutivo, Modelo.Consecutivo);
            return Resultado;
        }

        /// <summary>
        /// Metodo Helper para actualizar un registro de informacion investigador
        /// </summary>
        public static DataTable ActualizarInformacionInvestigadorUDT(ActualizarInformacionInvestigadorDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("LLF_IdInvestigador", typeof(System.Guid));
            Resultado.Columns.Add("LLF_IdEsepcialidad", typeof(System.Guid));
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Email", typeof(string));
            Resultado.Columns.Add("Direccion", typeof(string));
            Resultado.Columns.Add("Pais", typeof(string));

            Resultado.Rows.Add(Modelo.IdInvestigador, Modelo.IdEspecialidad, Modelo.Consecutivo, Modelo.Email, Modelo.Direccion, Modelo.Pais);
            return Resultado;
        }
        #endregion

        #region Metodos Extras UDT
        /// <summary>
        /// Metodo utilizado para la autenticacion de un usuario en concreto
        /// </summary>
        public static DataTable AutenticarUsuario(AccesoUsuarioDTO Modelo) {

            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Consecutivo", typeof(System.Guid));
            Resultado.Columns.Add("Usuario", typeof(string));
            Resultado.Columns.Add("Email", typeof(string));
            Resultado.Columns.Add("Password", typeof(string));

            Resultado.Rows.Add(null, Modelo.Usuario, null, null);
            return Resultado;
        }
        #endregion
    }
}
