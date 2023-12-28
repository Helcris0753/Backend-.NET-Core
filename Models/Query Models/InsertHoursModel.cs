namespace Backend.Models.Query_Models
{
    public class InsertHoursModel
    {
        public int ProfessorId { get; set; }
        public List<Hour> Hours { get; set; }
    }
    public class Hour
    {
        public string Hours { get; set; }
        public int DaysId { get; set; }
        public int ModalityID { get; set; }
    }
}
