using api_api_consumption;
using Backend.DBInteractions;
using Backend.Models.Alerts;
using Newtonsoft.Json;

namespace Backend.GetInformationForINTEC
{
    public class GetDataIntec
    {
        public static async Task<bool> GetDataForNTEC()
        {

            if (
                //se hace diferentes llamas al metodo ValidateInsertionsJsons con los diferntes Procedure que se van a
                //usar y las url de las endpoints.
                //Si hay algun error durante las llamdas a 'ValidateInsertionsJsons', se devolver false, en caso contrario true
                await ValidateInsertionsJsons<Pensums>("ppInsertInformationPensum", "EXAMPLE") &&
                await ValidateInsertionsJsons<Professors>("ppInsertInformationProfesors", "EXAMPLE") &&
                await ValidateInsertionsJsons<Sections>("ppInsertInformationSections", "EXAMPLE")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Este método procesa y guarda información proveniente de tres endpoints diferentes:
        // 1. EL que obtiene los pensums.
        // 2. El que obtiene la información básica de los profesores.
        // 3. El que obtiene la información de las secciones.
        // Esta información se guarda en la base de datos para su uso posterior en la aplicación.
        // Para que este método funcione, se requiere proporcionar un modelo (Pensums, Professors o Sections) que se guarda en la variable t,
        // el nombre del procedimiento almacenado y la URL del endpoint.
        private static async Task<bool> ValidateInsertionsJsons<t>(string StoreProcedure, string UrlEndpoint)
        {
            //lista de la clase que se pasa (Pensums, Professors, Sections)
            List<t> ObjectList = new List<t>();
            //Invocacion de la clase ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            //Invocacion del modelo ReturnMessage
            ReturnMessage message = new ReturnMessage();

            //Se obtiene el json que contiene toda la informacion necesario a partir de la url del endpoint
            string JsonAnswer = await GetJson(UrlEndpoint);

            //Si al obtener el json del endpoint se da un error este inmediatamente devuelve false, indicando
            //a GetDataforINTEC que no fue posible realizar la ejecucion. Dichos errores pueden se ocasionados por:
            //Que la api del INTEC no este encendida
            //Que se de un error durante la ejecucion de algun tipo
            if (JsonAnswer == "Error") { return false; }

            //Si no hay algun error, de deserializa el json en una lista del modelo pasato al metodo.
            ObjectList = JsonConvert.DeserializeObject<List<t>>(JsonAnswer);
            //se hace un recorrido sobre todo los objetos que componen la lista de objetos y se inserta cada objecto por
            //medio de la ExecuteStoredProcedure. 
            foreach (t item in ObjectList)
            {
                //Se le pasa el nombre del store procedure y el item en cuestion
                //si da ningun error en la insercion, se devuelve inmediatamente false
                if (ESP.Execute<object>(StoreProcedure, item) == null)
                {
                    return false;
                }
            }

            //si se ejecuta correctamente todo, se devuelve un true
            return true;
        }
        //este metodo tiene como objetivo obtener el json que devuelve el endpoint
        private static async Task<string> GetJson(string UrlEndpoint)
        {
            //se crea un cliente http
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //Se obtiene la respuesta de la consulta del endpoint 
                    HttpResponseMessage response = await client.GetAsync(UrlEndpoint);

                    //se verifica si la consulta fue exitosa y se devolvio informacion
                    if (response.IsSuccessStatusCode)
                    {
                        //si lo fue, se devuelve el json
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        //si no, se indica como un error
                        return "Error";
                    }

                }
                catch (Exception)
                {
                    //Si hay algun error, entonces durante la ejecucion tambien se devuelve 'Error'
                    return "Error";
                }
            }
        }

    }
}
