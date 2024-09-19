namespace SeniorLearn.WebApp.Data
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public virtual List<Member> Members { get; set; } = new();
        public int TimetableId { get; set; }
        public virtual Timetable Timetable { get; set; } = default!;
        public List<Topic> Topics { get; set; } = new();
    }
}
