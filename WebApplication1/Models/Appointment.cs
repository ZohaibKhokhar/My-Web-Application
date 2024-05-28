namespace WebApplication1.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string PetType { get; set; }
        public DateTime preferredDateTime { get; set; }
        public string Reason { get; set; }
    }
}
