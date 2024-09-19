namespace SeniorLearn.WebApp.Data
{
    public class Topic
    {
        public Topic()
        {
            
        }
        public Topic(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        //Organisation
        public int OrganisationId { get; set; } //foreignKey
        public Organisation Organisation { get; set; } = default!;
        public List<Lesson> Lessons { get; set; } = new();

    }
}
