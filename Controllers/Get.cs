using Backend.DBInteractions;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers
{
    [Route("Get")]
    [ApiController]
    public class Get : ControllerBase
    {
        //a cada metodo de a esta clase convierte los parametros que recibe en parametros para una variable de tio var.
        //en la misma, se almacenan los parametros y se envian al metodo de la clase ExecuteStoreProcedure junto con el nombre
        //del store procedure.
        [HttpGet]
        // [Authorize]
        [Route("ScheduleView")]
        public dynamic ScheduleView(string CarrerCode = "null", int Quarter = 0)
        {
            // Creación de un objeto ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            // Creación de los parámetros para el procedimiento almacenado
            var parameters = new { CarrerCode = CarrerCode == "null" ? DBNull.Value.ToString() : CarrerCode, Quarter = Quarter };
            // Llamada al procedimiento almacenado "ppGetScheduleView" con los parámetros definidos por medio del metodo de la clase
            string JSON = ESP.Execute<object>("ppGetScheduleView", parameters);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }

        [HttpGet]
        //[Authorize(Roles = "Coordinator,Administrator")]  
        [Route("CareerMaxQuarter")]
        public dynamic CareerMaxQuarter()
        {
            // Creación de un objeto ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            // Llamada al procedimiento almacenado "ppGetCareerMaxQuarter" con los parámetros definidos por medio del metodo de la clase
            // como en este caso, no se necesitan parametros, se para un parametro null
            string JSON = ESP.Execute<object>("ppGetCareerMaxQuarter", null);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }

        [HttpGet]
        //[Authorize]
        [Route("ProfessorSelection")]
        public dynamic ProfessorSelection(int ProfessorID = 0)
        {
            // Creación de un objeto ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            var parameters = new { ProfessorID = ProfessorID, Modality = 0 };
            string JSON = ESP.Execute<object>("ppGetProfessorSelection", parameters);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }

        [HttpGet]
        //[Authorize]
        [Route("ProfessorSchedule")]
        public dynamic ProfessorSchedule(int ProfessorID = 0)
        {
            // Creación de un objeto ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            var parameters = new { ProfessorId = ProfessorID };
            string JSON = ESP.Execute<object>("ppGetProfessorSchedule", parameters);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }

        [HttpGet]
        //  [Authorize(Roles = "Coordinator,Administrator")]
        [Route("SectionsView")]
        public dynamic SectionsView(string SubjectCode)
        {
            // Creación de un objeto ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            var parameters = new { SubjectCode = SubjectCode == "all" ? DBNull.Value.ToString() : SubjectCode.ToUpper() };
            string JSON = ESP.Execute<object>("ppGetSectionsView", parameters);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }

        [HttpGet]
        //  [Authorize (Roles = "Coordinator,Administrator")]
        [Route("SubjectView")]
        public dynamic SubjectView()
        {
            // Creación de un objeto ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            string JSON = ESP.Execute<object>("ppGetSubjectView", null);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }

        [HttpGet]
        //  [Authorize (Roles = "Coordinator")]
        [Route("VerifyProfessor")]
        public dynamic VerifyProfessor(int ProfessorId)
        {
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            var Parameters = new { ProfessorId = ProfessorId };
            string JSON = ESP.Execute<object>("ppGetVerifyProfessor", Parameters);

            return JSON != null ?
                Ok(JSON) :
                StatusCode(300);
        }
    }
}
