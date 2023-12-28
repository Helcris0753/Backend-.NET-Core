using Autohorario.Models;
using Backend.DBInteractions;
using Backend.Models.Alerts;
using Backend.Models.Query_Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("Put")]
    [ApiController]
    //Se le asigna a la clase put que here caracteristicas de la clase controller
    public class Put : Controller
    {
        // Esta es una clase que tiene un constructor llamado "Put" que recibe un objeto IConfiguration como parámetro.
        private IConfiguration _configuration;

        // Constructor de la clase "Put" que inicializa la variable privada "_configuration" con la configuración proporcionada.
        public Put(IConfiguration configuration)
        {
            _configuration = configuration; // Aquí se asigna la configuración proporcionada al campo "_configuration".
        }

        [HttpPut]
        [Route("Login")]
        public dynamic Login([FromBody] string Token)
        {

            Authentication auth = new Authentication(_configuration);
            string token = auth.Decorekey(Token);

            return token != "Not found" ? Ok(token) : NotFound("Invalid user");
        }
        [HttpPut]
        // [Authorize(Roles = "Coordinator,Administrator")]
        [Route("UpdateHours")]
        // La lógica subyacente aquí implica que, al proporcionar el ID de una hora y establecer el día en 0, se eliminará esa hora específica.
        // Si se proporciona el día de la hora junto con un ID de día diferente de sera y entre 1 a 6 (representando los dias de la semana hasta el sabado), se eliminará la hora previa y se creará una nueva hora en su lugar, efectivamente actualizándola.
        // Si se establece el día de la hora en 0, se creará una nueva hora.
        public dynamic UpdateHours(HourModel HourParameters)
        {
            //Nueva instancia de la clase ExecuteStoreProcedure
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            List<int> Days = new List<int> { 1, 2, 3, 4, 5, 6};

            //si el HourId es diferente de 0, esa hora asociada a ese id se elimina
            if (HourParameters.HourId != 0)
            {
                var DeleteParameters = new
                {
                    HourId = HourParameters.HourId
                };

                if (ESP.Execute<object>("ppDeleteHour", DeleteParameters) == null) 
                    return StatusCode(300);
            }

            //si el dia el dia es es diferente de 0 de antonces la hora se crea
            if (Days.Contains(HourParameters.Day))
            {
                var InsertParameters = new
                {
                    Hours = HourParameters.Hour,
                    Day = HourParameters.Day,
                    SectionId = HourParameters.SectionId,
                    HourState = HourParameters.ValidateStatusHour(),
                    Modality = HourParameters.Modality
                };

                if(ESP.Execute<object>("ppInsertHours", InsertParameters) == null) 
                    return StatusCode(300);
            }

            return Ok("Successfully");

        }
        [HttpPut]
        // [Authorize]
        [Route("InsertSelectedHours")]
        public dynamic InsertHours(InsertHoursModel ObjectHours)
        {
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            foreach (Hour hour in ObjectHours.Hours)
            {
                if (!(int.TryParse(hour.Hours.Substring(0,2), out _) || int.TryParse(hour.Hours.Substring(3, 2), out _)))
                {
                    return BadRequest();
                }
            }
            //Se crea a variable var con el id del profesor de contenida en el objeto del modelo que se va a pasar
            var Parameter = new { ProfessorId = ObjectHours.ProfessorId };

            //Se ejecuta el procedure para eliminar todas las otras que tiene el profesor
            if(ESP.Execute<object>("ppDeleteSelectedHours", Parameter) == null)
                return StatusCode(300);

            //foreach que recorre las horas contenidas en el objeto 'ObjectHours'
            foreach (Hour hour in ObjectHours.Hours)
            {

                //se crea un objecto  de ripo var con diferentes parametros del objectp 'ObjectHours'
                var Parameters = new
                {
                    ProfessorId = ObjectHours.ProfessorId,
                    Hour = hour.Hours,
                    DayId = hour.DaysId,
                    ModalityId = hour.ModalityID
                };

                //Estos parametrso se le pasan al metodo para ejecutar el store procedure 'ppInsertSelectedHours'
                // y se le pasan los paramtros requerrido para ejecutar el store procedure
                //Si el errorbanner no es full, entonces inidica un error, por lo que se hace un statuscode con codigo 300
                if (ESP.Execute<object>("ppInsertSelectedHours", Parameters) == null)
                    return StatusCode(300);
            }

            //si el foreach se completa con exito, entonces se llega hasta aqui y se devuelve un mesaje de exito
            return Ok("Success");

        }
    }
}