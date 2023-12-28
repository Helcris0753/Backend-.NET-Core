using Autohorario.Models;
using Backend.DBInteractions;
using System.Diagnostics;

namespace Autohorario
{
    internal class Obtaining
    {
        internal static ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
        public static int Run()
        {
            //Listas del objeto Hours  quesecompone de estamanera: (Hour, Day)
            List<Hours> WeeklyScheduleAvailable = new List<Hours>();
            List<Hours> SelectOnsiteSchedule = new List<Hours>();
            List<Hours> SelectVirtualSchedule = new List<Hours>();

            List<InformationForDB> Info = new List<InformationForDB>();
            //clase para medir el tiempo de ejecucion del programa
            Stopwatch stopwatch = new Stopwatch();
            //Se inicia el conteo de los milisegundos
            stopwatch.Start();

            // Se ejecuta una consulta a la base de datos utilizando Dapper
            // y se almacena el resultado en la variable 'Info'.
            // No se están pasando parámetros en este caso. La consulta se hace como base del modelo 'InformationForDB'
            Info = new List<InformationForDB>(ESP.Execute<InformationForDB>("ppGetInformacion", null, false));

            // Para cada elemento en 'Info', se ejecutan ciertas operaciones.
            foreach (var item in Info)
            {
                // Se obtiene el horario presencial (onsite) para el profesor actual
                // utilizando el método 'GetSelectSchedule' con la modalidad 1 (presencial).
                SelectOnsiteSchedule = GetSelectSchedule(item.ProfessorId, 1);
                // Se obtiene el horario virtual para el profesor actual
                // utilizando el método 'GetSelectSchedule' con la modalidad 2 (virtual).
                SelectVirtualSchedule = GetSelectSchedule(item.ProfessorId, 2);

                // Se crea una nueva lista 'WeeklyScheduleAvailable' basada en 'WeeklyWorkSchedule'.
                WeeklyScheduleAvailable = CreateWeeklySchedule();
                // Se pasan varios parámetros relacionados con el profesor actual, suseccion, su asignatura y sus horarios.
                Validation.Getdata(item.SubjectId, item.SectionId, item.SubjectCredits, item.ModalityId, item.ProfessorId, SelectOnsiteSchedule, SelectVirtualSchedule, WeeklyScheduleAvailable);
                SelectOnsiteSchedule.Clear();
                SelectVirtualSchedule.Clear();
                WeeklyScheduleAvailable.Clear();
            }

            //secierra la coneccion y se detiene stopwatch
            stopwatch.Stop();
            return int.Parse(stopwatch.ElapsedMilliseconds.ToString()) / 1000;
        }
        //metdo que devuelve las horas seleccionadas de un profesorsegun su id y su modalidad
        internal static List<Hours> GetSelectSchedule(int ProfessorId, int Modality)
        {
            //lista que guardara el horario seleccionado por el profesor
            List<Hours> Select_Schedule = new List<Hours>();
            //Parametros a pasar para el store procedure
            var Parameters = new
            {
                ProfessorId = ProfessorId,
                Modality = Modality
            };
            //consulta al store procedure 'ppGetProfessorSelection' en la base de datos, al cual se le pasa parametros
            //y devuelve una lista de objetos de tipo hora.

            Select_Schedule = new List<Hours>( ESP.Execute<Hours>("ppGetProfessorSelection", Parameters, false));

            return Select_Schedule;
        }
        internal static List<Hours> CreateWeeklySchedule()
        {
            List<Hours> WeeklyWorkSchedule = new List<Hours>();
            for (int i = 1; i <= 6; i++)
            {
                WeeklyWorkSchedule.Add(new Hours { Hour = i != 6 ? "07/22" : "07/18", Day = i });
            }
            return WeeklyWorkSchedule;
        }
    }
}
