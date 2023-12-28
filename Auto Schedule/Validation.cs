using Autohorario.Models;
using System.Data;

namespace Autohorario
{
    internal class Validation
    {

        internal static void Getdata(int SubjectId, int SectionId, int SubjectCredits, int Modality, int ProfessorId, List<Hours> SelectOnsiteSchedule, List<Hours> SelectVirtualSchedule, List<Hours> WeeklySchedule)
        {
            //Diferentes listas de del modelo Hous cuyo nombre indica su proposito
            List<Hours> AvailableOnsiteSchedule = new List<Hours>();
            List<Hours> AvailableVirtualSchedule = new List<Hours>();
            List<Hours> AvailableWeeklySchedule = new List<Hours>();
            List<Hours> SelectedHours = new List<Hours>();
            //union de los horarios seleccionados 
            SelectedHours = SelectOnsiteSchedule.Concat(SelectVirtualSchedule).ToList();

            //Validacion de las horas segun el tipo de horario.
            AvailableOnsiteSchedule = ValidateHours(SubjectId, ProfessorId, SelectOnsiteSchedule).ToList(); //presencial
            AvailableVirtualSchedule = ValidateHours(SubjectId, ProfessorId, SelectVirtualSchedule).ToList(); //virtual
            AvailableWeeklySchedule = ValidateHours(SubjectId, ProfessorId, WeeklySchedule).ToList(); //disponible semanal

            Insertion.GetDataforInsertion(SectionId, SubjectCredits, Modality, AvailableOnsiteSchedule, AvailableVirtualSchedule, AvailableWeeklySchedule, SelectedHours);
        }
        //metodo que valida las horas seleccionadas con las horas ocupadas
        internal static List<Hours> ValidateHours(int SubjectId, int ProfessorId, List<Hours> Schedule, int SectionId = 0)
        {
            //variables que se van a utilizar
            string HourInstance;
            int SelectionHourStart, SelectionHourEnd, SubjectHourStart, SubjectHourEnd, Day;
            List<Hours> AvailableSchedule = new List<Hours>();
            List<Hours> NonAvailableSchedule = new List<Hours>();
            List<Hours> SelectSchedule = new List<Hours>(Schedule);

            //parametros para el store procedure necsarios para quenos devvuelva las horas ocuapdas segun el id del profesor y la section del mismo
            var Parameters = new
            {
                SubjectId = SubjectId,
                ProfessorId = ProfessorId,
                HourId = 0,
                SectionId = SectionId
            };
            // Se ejecuta una consulta a la base de datos utilizando Dapper
            //y se guardan en la variable NonAvailableSchedule.
            //Alli se almacenan las horas ocupadas 

            NonAvailableSchedule = new List<Hours>(Obtaining.ESP.Execute<Hours>("ppGetCheckSchedule", Parameters, false));

            NonAvailableSchedule.Distinct();

            //si no hay horas ocupadas segun los parametros, entonces el horario seleccionado se convierte en el disponible
            if (NonAvailableSchedule.Count == 0)
            {
                AvailableSchedule.AddRange(SelectSchedule);
            }

            //se hace un for de las horas seleccionadas y su dia
            for (int i = 0; i < SelectSchedule.Count; i++)
            {
                //Se guarda el dia i de la seleccion 
                Day = SelectSchedule[i].Day;
                //se hace un for de las horas ocupadas
                for (int j = 0; j < NonAvailableSchedule.Count; j++)
                {
                    //la nstancia hora es el intervalo de hora de horario seleccionado para el registro i
                    HourInstance = SelectSchedule[i].Hour;
                    //la hora inicio y final del intervalo contenido en instancia hora
                    SelectionHourStart = int.Parse(HourInstance.Substring(0, 2));
                    SelectionHourEnd = int.Parse(HourInstance.Substring(3, 2));
                    //la hora inicio y final del intervalo contenido en el registro j
                    SubjectHourStart = int.Parse(NonAvailableSchedule[j].Hour.Substring(0, 2));
                    SubjectHourEnd = int.Parse(NonAvailableSchedule[j].Hour.Substring(3, 2));
                    //si los Days de el horario disponible y Hours ocupadas son iguales, se entrara al if
                    if (SelectSchedule[i].Day == NonAvailableSchedule[j].Day)
                    {
                        //Si hay algun solapamiento de horas
                        if (SubjectHourStart <= SelectionHourEnd || SubjectHourEnd >= SelectionHourStart)
                        {
                            //si las Hours ocupadas se solapan de tal modo:
                            //      __________ (Hours seleccionadas)
                            //  _________      (Hours ocupadas)
                            // Se tomara tan solo las Hours que van de desde el fin de las Hours ocupadas hasta el fin de las Hours seleccionadas
                            // Se toma en cuenta cuando las Hours de fin da ambos intervalos son iguales
                            if (SelectionHourEnd > SubjectHourEnd && SelectionHourEnd > SubjectHourStart && SubjectHourEnd > SelectionHourStart && SelectionHourStart >= SubjectHourStart)
                            {
                                //el registro de i se modifica segun los requerimientos del if
                                //insercion.zero es un metodo que viene de la clase insercion que sirve para colocar un zero antes de numero si numero es menor a 10
                                SelectSchedule[i].Hour = $"{Insertion.zero(SubjectHourEnd)}/{Insertion.zero(SelectionHourEnd)}";
                                SelectSchedule[i].Day = Day;
                            }
                            //si las Hours ocupadas se solapan de tal modo:
                            // __________       (Hours seleccionadas)
                            //      _________   (Hours ocupadas)
                            // Se tomara tan solo las Hours que van de desde el inicio de las Hours seleccionadas hasta el inicio de las Hours ocupadas
                            //Se toma en cuenta cuando las Hours de inicio da ambos intervalos son iguales
                            else if (SubjectHourEnd >= SelectionHourEnd && SubjectHourEnd > SelectionHourStart && SelectionHourEnd > SubjectHourStart && SubjectHourStart > SelectionHourStart)
                            {
                                SelectSchedule[i].Hour = $"{Insertion.zero(SelectionHourStart)}/{Insertion.zero(SubjectHourStart)}";
                                SelectSchedule[i].Day = Day;
                            }
                            //si las Hours ocupadas se solapan de tal modo:
                            // ___________________       (Hours seleccionadas)
                            //      _________           (Hours ocupadas)
                            //Se tomaran las Hours que van desde el inicio de las Hours selecciones hasta el de las Hours ocupadas
                            //Las Hours que van desde el fin de las Hours ocupadas hasta el final de las seleccionadas se insertan en horario seleccionado
                            else if (SelectionHourStart < SubjectHourStart && SubjectHourEnd < SelectionHourEnd)
                            {

                                SelectSchedule[i].Hour = $"{Insertion.zero(SelectionHourStart)}/{Insertion.zero(SubjectHourStart)}";
                                SelectSchedule[i].Day = Day;
                                SelectSchedule.Add(
                                    new Hours
                                    {
                                        Hour = $"{Insertion.zero(SubjectHourEnd)}/{Insertion.zero(SelectionHourEnd)}",
                                        Day = Day
                                    });
                            }
                            //si no hay espacio disponible para la hora, vease en el ejemplo, se opta por poner la hora 00/00 y se elimina mas adelante:
                            //    __________      (Hours seleccionadas)
                            // __________________ (Hours ocupadas)
                            else if (SelectionHourStart >= SubjectHourStart && SelectionHourEnd <= SubjectHourEnd)
                            {
                                SelectSchedule[i].Hour = "00/00";
                                SelectSchedule[i].Day = Day;
                            }
                        }
                    }
                }
            }

            //se hace la distincion en este for de que a partir de las 12 del medio dia, las horas deben de empezar par y terminar par en la mayotia de casos
            SelectSchedule = SelectSchedule.OrderBy(x => x.Day).ToList();
            for (int i = 0; i < SelectSchedule.Count; i++)
            {
                SubjectHourStart = int.Parse(SelectSchedule[i].Hour.Substring(0, 2));
                SubjectHourEnd = int.Parse(SelectSchedule[i].Hour.Substring(3, 2));
                if (SubjectHourStart > 13 && SubjectHourStart % 2 != 0 && SubjectHourEnd - SubjectHourStart > 1)
                {
                    SelectSchedule[i].Hour = $"{SubjectHourStart + 1}/{SubjectHourEnd}";
                }
            }
            //se eliminan horas cuya logitud sea igual o menor a cero, vease ("00/00"), no es mayor que 1.
            AvailableSchedule = SelectSchedule.Where(horario => int.Parse(horario.Hour.Substring(3, 2)) - int.Parse(horario.Hour.Substring(0, 2)) > 0).ToList();
            //Se eliminan las horas que coincidan entre el AvailableSchedule y el NonAvailableSchedule
            //AvailableSchedule = AvailableSchedule.Except(NonAvailableSchedule).ToList();

            return AvailableSchedule;
        }
    }
}
