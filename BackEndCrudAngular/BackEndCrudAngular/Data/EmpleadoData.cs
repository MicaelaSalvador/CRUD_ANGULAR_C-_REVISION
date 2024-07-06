using BackEndCrudAngular.Models;
using System.Data;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace BackEndCrudAngular.Data
{
    public class EmpleadoData
    {
        private readonly string conexion;

        public EmpleadoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSql")!;
        }
        #region Listar Empleado
        public async Task<List<Empleado>> Lista()
        {
            List<Empleado> lista = new List<Empleado>();

            using (var con = new MySqlConnection(conexion))
            {
                await con.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("sp_listaEmpleados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        #endregion

        #region Obtener Empleado
        public async Task<Empleado> obtener(int Id)
        {
            Empleado objeto = new Empleado();

            using (var con = new MySqlConnection(conexion))
            {
                await con.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("sp_obtenerEmpleados", con);
                cmd.Parameters.AddWithValue("pIdEmpleado", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString()
                        };
                    }
                }
            }
            return objeto;
        }
        #endregion

        #region crear Empleado
        public async Task<bool> Crear(Empleado objeto)
        {
            bool respuesta = true;
            using (var con = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand("sp_crearEmpleado", con);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                { 
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        #endregion

        #region Editar Empleado
        public async Task<bool> editar(Empleado objeto)
        {
            bool respuesta = true;

            using (var con = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand("sp_editarEmpleado", con);
                cmd.Parameters.AddWithValue("pIdEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("pNombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("pCorreo", objeto.Correo);
                cmd.Parameters.AddWithValue("pSueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("pFechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;

                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        #endregion

        #region Eliminar Empleado

        public async Task<bool> eliminar(int id)
        {
            bool respuesta = true;

            using (var con = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand("sp_eliminarEmpleado", con);
                cmd.Parameters.AddWithValue("pIdEmpleado", id);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        #endregion
    }
}
