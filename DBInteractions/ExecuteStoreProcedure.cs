using Dapper;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static Dapper.SqlMapper;

namespace Backend.DBInteractions
{
    public class ExecuteStoreProcedure
    {
        //Variable de la clase que indica la coneccion a la misma, para obtener el conection string se reqquiere del
        //metodo GetConnectionString
        private SqlConnection connection = new SqlConnection(GetConnectionString());

        //este metodo extrae el conection string del archivo de configuracion del aplicativo
        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            return configuration.GetConnectionString("Local");
        }        

        internal dynamic Execute<T>(string ProcedureName, object Parameters, bool JSON = true)
        {
            //Lista del modelo pasado al motodo
            List<T> Result = new List<T>();
            //Para abrir la coneccio a la base de datos y interractuar con ella
            try
            {
                //Se ejecuta el metodo para conectarnos o desconectarnos de la base de datos
                connection.Open();

                //microservicio de dapper para ejecutar un store procedure, para ejecutar esto se necesita del nombre del store
                //procedure, los parametros y el tipo de comando.

                // Se ejecuta una consulta en la base de datos utilizando Dapper y se almacena el resultado en 'Result'
                Result = connection.Query<T>(
                    ProcedureName,         // Nombre de la stored procedure a ejecutar
                    Parameters,            // Parámetros que se pasan a la stored procedure
                    commandType: CommandType.StoredProcedure // Tipo de comando, en este caso, se indica que es una stored procedure
                ).ToList();               // Convierte el resultado de la consulta en una lista
            }
            catch (Exception)
            {
                return JSON ? null : new List<T>();
            }
            finally
            {
                connection.Close();
            }

            if (JSON)
            {
                return Result == null ?
                    "Successfully" :
                    JsonConvert.SerializeObject(Result);
            }
            else
            {
                return Result.Any() ?
                    Result : 
                    new List<T>();
            }

        }
    }
}
