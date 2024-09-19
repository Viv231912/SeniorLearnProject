using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Administration.Models.Topic
{
    public class Create
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; } = default!;


    }
}
