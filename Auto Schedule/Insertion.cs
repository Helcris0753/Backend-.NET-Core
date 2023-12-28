using Autohorario.Models;

namespace Autohorario
{
    internal class Insertion
    {
        static private Random random = new Random();

        public static void GetDataforInsertion(int SectionId, int SubjectCredits, int Modality, List<Hours> OnsideSchedule, List<Hours> VirtualSchedule, List<Hours> WeeklyScheduleAvailable, List<Hours> SelectSchedule)
        {
            //variables que se van a usar
            List<(bool, int)> InsertionValid = new List<(bool, int)>();
            List<Hours> Null = new List<Hours>();
            Hours RHour = new Hours();
            Null.Add(new Hours { Hour = "00/00", Day = 0 });
            List<List<Hours>> ScheduleColection = new List<List<Hours>>();
            //Se añaden las diferentes lista de horas al ScheduleColection
            ScheduleColection.Add(OnsideSchedule);
            ScheduleColection.Add(VirtualSchedule);
            ScheduleColection.Add(WeeklyScheduleAvailable);
            ScheduleColection.Add(SelectSchedule);
            int Day1 = 0, Day2 = 0, Day3 = 0;
            //las inserciones de las asignaturas dependeran de sus creditos.
            switch (SubjectCredits)
            {
                //las asignaturas de 0 a 2 creditos requieren metodos de insercion iguales
                case 0:
                case 1:
                case 2:
                    //validate hour devuelve el dia en el que se insertó la hora, si es 0, entonces no se puedo insertar hora alguna, por lo que se
                    //generara una hora aleatoria.
                    if (ValidateSchedule(ScheduleColection, SectionId, 2, Modality) == 0)
                    {
                        //genera una hora y un dia aleatorio
                        RHour = RandomHour(2);
                        InsertionToDB(RHour.Day, SectionId, 3, Modality, RHour.Hour);
                    }
                    break;
                case 3:
                    //Es el mismo proceso que el anterior, por con tres creditos
                    if (ValidateSchedule(ScheduleColection, SectionId, 3, Modality) == 0)
                    {
                        RHour = RandomHour(3);
                        InsertionToDB(RHour.Day, SectionId, 3, Modality, RHour.Hour);
                    }
                    break;
                case 4:
                case 5:
                    //aqui se selecciona la modalidad de la seccion
                    switch (Modality)
                    {
                        case 1:
                        case 2:
                            //como 'ValidateSchedule' devuelve el dia en la que se insertó la hora, este mismo se guarda en una variable
                            //para futuramente hacer que este dia no coincida con la proxima insercionn
                            Day1 = ValidateSchedule(ScheduleColection, SectionId, 2, Modality);
                            //si el day1 es 0, entonces se coloca un dia y hora hora aleatoria para dia
                            if (Day1 == 0)
                            {
                                //se le pasa la logintud de las horas aleatorias. 
                                RHour = RandomHour(2);
                                Day1 = RHour.Day;
                                //se inserta directamente en la base de datos
                                InsertionToDB(Day1, SectionId, 3, Modality, RHour.Hour);
                            }
                            //es lo mismo que el procesos anterior, tan solo con el dia 2
                            Day2 = ValidateSchedule(ScheduleColection, SectionId, 2, Modality, Day1);
                            if (Day2 == 0)
                            {
                                //aqui se le pasa otro parametro, que es el dia 1, para que lo que devuelva no tenga el dia similar
                                RHour = RandomHour(2, Day1);
                                Day2 = RHour.Day;
                                InsertionToDB(Day2, SectionId, 3, Modality, RHour.Hour);
                            }

                            //si la asignatura es de 5 creditos, entnces hay que colocar un 
                            if (SubjectCredits == 5)
                                Day3 = ValidateSchedule(ScheduleColection, SectionId, 1, Modality, Day1, Day2);
                            break;
                        //en caso de que la modalidad sea hibrida (3)
                        case 3:
                            //primero se prueba con las horas presenciales seleccionadas, y las otras se colocan nulas.
                            //una lista de horas nula consiste de la siguiente forma: { Hour = "00/00", Day = 0 }
                            ScheduleColection[1] = Null;
                            ScheduleColection[2] = Null;
                            ScheduleColection[3] = Null;

                            Day1 = ValidateSchedule(ScheduleColection, SectionId, 2, 1);   //se valida con solo el horario presencial y se coloca la modalidad de la hora presencial (1)

                            if (Day1 == 0)    //si no se pudo insertar con el horario presencial, se hace con el horario virtual
                            {
                                ScheduleColection[0] = Null;
                                ScheduleColection[1] = VirtualSchedule;

                                Day1 = ValidateSchedule(ScheduleColection, SectionId, 2, 2);   //se valida con solo el horario virtual y se coloca la modalidad de la hora virtual (2)
                                if (Day1 == 0) //si no se pudo insertar con el horario virtual, se hace con los horarios weekscheduleavailable y selectschedule
                                {
                                    ScheduleColection[1] = Null;
                                    ScheduleColection[2] = WeeklyScheduleAvailable;
                                    ScheduleColection[3] = SelectSchedule;

                                    Day1 = ValidateSchedule(ScheduleColection, SectionId, 2, 1);
                                    Day2 = ValidateSchedule(ScheduleColection, SectionId, 2, 2, Day1);
                                }
                                else // si se pudo insertar un dia con el horario virtual, entonces se coloca el siguiente dia de la seccion
                                //hibrida como presencial y se colocan los horarios weekscheduleavailable y selectschedule
                                {
                                    ScheduleColection[2] = WeeklyScheduleAvailable;
                                    ScheduleColection[3] = SelectSchedule;

                                    Day2 = ValidateSchedule(ScheduleColection, SectionId, 2, 2, Day1);
                                }
                            }
                            else //si se inverto segun el horario presencial, el dia 2 se coloca segun el horario virtual
                            {
                                ScheduleColection[0] = Null;
                                ScheduleColection[1] = VirtualSchedule;
                                Day2 = ValidateSchedule(ScheduleColection, SectionId, 2, 2, Day1);
                                if (Day2 == 0) //si no se puedo invertar segun el horario virtual se hace por medio del horario presencial, weekscheduleavailable y selectschedule
                                {

                                    ScheduleColection[0] = OnsideSchedule;
                                    ScheduleColection[2] = WeeklyScheduleAvailable;
                                    ScheduleColection[3] = SelectSchedule;

                                    Day2 = ValidateSchedule(ScheduleColection, SectionId, 2, 2, Day1);
                                }
                            }

                            //si alguno de los dias sigue siendo 0, entonces las horas seran aleatorias
                            if (Day1 == 0)
                            {
                                RHour = RandomHour(2);
                                InsertionToDB(RHour.Day, SectionId, 3, Modality, RHour.Hour);
                                Day1 = RHour.Day;
                            }
                            if (Day2 == 0)
                            {
                                RHour = RandomHour(2, Day1);
                                InsertionToDB(RHour.Day, SectionId, 3, Modality, RHour.Hour);
                                Day2 = RHour.Day;
                            }
                            if (SubjectCredits == 5)
                                Day3 = ValidateSchedule(ScheduleColection, SectionId, 1, Modality, Day1, Day2);
                            break;
                    }
                    break;
                default:
                    InsertionToDB(1, SectionId, 3, Modality);
                    break;
            }
        }

        //metodo que se encarga de validar los horarios
        private static int ValidateSchedule(List<List<Hours>> ScheduleColection, int SectionId, int Hours, int Modality, int Day1 = 0, int Day2 = 0)
        {
            //variables a usar 
            List<Hours> Schedule = new List<Hours>();
            int InsertionDay = 0;
            int count = ScheduleColection.Count;
            int HourState;

            //se hace un for para recorrer los horarios contenidos en la lista de horarios 
            for (int i = 0; i < count; i++)
            {
                //se hace una instancia de dicha horario
                Schedule = ScheduleColection[i];

                // HourState se calcula basándose en la modalidad.
                // Para la modalidad presencial (1):
                // El primer horario tiene un estado verde (1),
                // los horarios intermedios tienen un estado amarillo (2),
                // y el último horario tiene un estado rojo (3).
                // Para la modalidad virtual (2):
                // El segundo horario tiene un estado verde (1),
                // los horarios antes y después del segundo tienen un estado amarillo (2),
                // y el último horario tiene un estado rojo (3).
                HourState = (i == Modality - 1) ? 1 : (i == count) ? 3 : 2;

                //se comprueban los parametros por medio de ValidateInsertion
                InsertionDay = ValidateInsertion(Schedule, SectionId, Hours, HourState, Modality, Day1, Day2);

                //Validate tiene dos campos, validation y day, validation indica que si se inserto con ese horario
                //si no se inserto con ese horario, entonces continua al siguiente
                if (InsertionDay == 0)
                {
                    continue;
                }

                //el siguiente campo indica el dia e el que se inserto el dia y este se devuelve.
                return InsertionDay;

            }
            return 0;
        }
        //metodo que sirve para validar las horas si cumplen con la cantidad de horas necesarias para alojar una seccion de una
        //asignatura segun sus creditos
        private static int ValidateInsertion(List<Hours> AuxiliarSchedule, int SectionId, int Hours, int HourStates, int Modality, int Day1 = 0, int Day2 = 0)
        {
            //variables a usar
            List<Hours> Schedule = AuxiliarSchedule;
            int StartHour, EndHour;

            //for que pasa por cada item en el horario que se pasa
            for (int i = 0; i < Schedule.Count; i++)
            {
                //se toman las horas de inicio y fin convirtiendolas en int.
                StartHour = int.Parse(Schedule[i].Hour.Substring(0, 2));
                EndHour = int.Parse(Schedule[i].Hour.Substring(3, 2));
                //si las horas tienes un estado de horas rojo (3), entonces las horas son tomadas aleatoriamente del horario 'SelectSchedule'
                if (HourStates == 3)
                {
                    int StartRandom = 0, EndRandom = 0;
                    if ((EndHour - StartHour) >= Hours && Schedule[i].Day != Day1 && Schedule[i].Day != Day2)
                    {
                        do
                        {
                            StartRandom = random.Next(StartHour, EndHour);
                            EndRandom = random.Next(StartRandom, EndHour + 1);
                        } while (EndRandom - StartRandom < Hours);
                        InsertionToDB(Schedule[i].Day, SectionId, HourStates, Modality, $"{zero(StartRandom)}/{zero(StartRandom + Hours)}");
                        return Schedule[i].Day;
                    }
                }
                else
                {
                    //si no tiene un estado de hora rojo (3), y cumple con los requisitos debajo, entonces tan solo se inserta por medio
                    //del metodo 'InsertionToDB'
                    if ((EndHour - StartHour) >= Hours && Schedule[i].Day != Day1 && Schedule[i].Day != Day2)
                    {
                        InsertionToDB(Schedule[i].Day, SectionId, HourStates, Modality, $"{zero(StartHour)}/{zero(StartHour + Hours)}"); //solo se añaden las horas necesarias al StartHour
                        return Schedule[i].Day;
                    }
                }
            }
            return 0;
        }
        //metodo que sirve para insertar en la base de datos las horas generadas
        private static void InsertionToDB(int Day, int SectionId, int HourState, int Modality, string Hours = "null")
        {
            //Se crean los parametros necesarios para pasarselos al store procedure por medio deun microservcio de dapper
            var Parameters = new
            {
                Hours = Hours != "null" ? Hours : (object)DBNull.Value, //Si las horas son nulas, estas se convierten en un objeto de tipo DBNull.Value
                Day = Day,
                SectionId = SectionId,
                HourState = HourState,
                Modality = Modality
            };

            Obtaining.ESP.Execute<object>("ppInsertHours", Parameters, false);

        }
        //Metodo que devuelve quesi un numero es menor a 10, a este se le coloca un cero delante
        public static string zero(int Hour)
        {
            return Hour < 10 ? "0" + Hour : Hour.ToString();
        }
        //metodo generador de horas aleatorias
        private static Hours RandomHour(int SubjectHours, int ExcludeDay = 0)
        {
            Hours hours = new Hours();
            Random random = new Random();
            int StartHour, EndHour, Day;

            //so el exclude dia no se pasa, entonces se day toma un valor aleatorio
            if (ExcludeDay == 0)
            {
                Day = random.Next(1, 7);
            }
            else
            { //cuando se pasa un exclude day, significa que day no puede tomar su mismo valor
                do //para generar un numero aleatorio que no se igual al exclude
                {
                    Day = random.Next(1, 7);

                } while (Day == ExcludeDay);
            }

            //se genera una starthour iniciando a las 7 y concluyendo segun el dia de semana. Por lo general los sabados terminan 
            //las clases a las 6 de la tarde
            StartHour = random.Next(7, Day == 6 ? 15 : 21);
            //SI SubjectHours es 3 y y se genero una hora aleatoria a las 20, esto significa que la asignatura terminara a las 23.
            //esto no es correcto, por lo que para esto esta esta validacion.
            if ((StartHour == 16 || StartHour == 20) && SubjectHours > 2)
            {
                StartHour = StartHour - 1;
                EndHour = StartHour + 3;
            }
            else
            {
                EndHour = StartHour + SubjectHours;
            }

            //se le asignan valores a los componentes del modelohours.
            hours.Hour = $"{zero(StartHour)}/{zero(EndHour)}";
            hours.Day = Day;

            return hours;
        }
    }
}
