namespace SeniorLearn.WebApp.Data.Identity
{
    public class RoleUpdate
    {
        public int Id { get; set; }
        public bool Active {  get; set; }
        public string Notes { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public DateTime RenewalDate { get; set; }
       
    }
}
