using Autohorario;
using Autohorario.Models;
using Backend.DBInteractions;

namespace Backend.Models.Query_Models
{
    public class HourModel
    {
        public int HourId { get; set; }
        public int ProfessorId { get; set; }
        public int SubjectId { get; set; }
        public string Hour { get; set; }
        public int Day { get; set; }
        public int Modality { get; set; }
        public int SectionId { get; set; }

        public int ValidateStatusHour()
        {
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();
            List<List<Hours>> SchedulesCollection = new List<List<Hours>>();
            List<Hours> Schedule = new List<Hours>();
            int HourState = 3, ScheduleStartHour, ScheduleEndHour, StartHour, EndHour, HourLong;

            List<Hours> OnsideSchedule = Obtaining.GetSelectSchedule(ProfessorId, 1);
            OnsideSchedule = Validation.ValidateHours(SubjectId, ProfessorId, OnsideSchedule, SectionId);

            List<Hours> VirtualSchedule = Obtaining.GetSelectSchedule(ProfessorId, 2);
            VirtualSchedule = Validation.ValidateHours(SubjectId, ProfessorId, VirtualSchedule, SectionId);

            List<Hours> WeeklySchedule = Obtaining.CreateWeeklySchedule();
            WeeklySchedule = Validation.ValidateHours(SubjectId, ProfessorId, WeeklySchedule, SectionId);

            SchedulesCollection.Add(OnsideSchedule);
            SchedulesCollection.Add(VirtualSchedule);
            SchedulesCollection.Add(WeeklySchedule);

            StartHour = int.Parse(Hour.Substring(0, 2));
            EndHour = int.Parse(Hour.Substring(3, 2));


            HourLong = int.Parse(Hour.Substring(3, 2)) - int.Parse(Hour.Substring(0, 2));
            for (int i = 0; i < SchedulesCollection.Count; i++)
            {
                Schedule = SchedulesCollection[i];
                HourState = (i == Modality - 1) ? 1 : 2;


                for (int j = 0; j < Schedule.Count; j++)
                {
                    if (Schedule[j].Day == Day)
                    {
                        ScheduleStartHour = int.Parse(Schedule[j].Hour.Substring(0, 2));
                        ScheduleEndHour = int.Parse(Schedule[j].Hour.Substring(3, 2));
                        if ((StartHour <= ScheduleStartHour || ScheduleEndHour >= EndHour) && (StartHour >= ScheduleStartHour && ScheduleEndHour >= EndHour))
                        {
                            return HourState;
                        }
                    }
                }
            }
            return 3;

        }
    }
}
