namespace MiniUdemy.Models
{
    public class TeacherRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Qualifications { get; set; } // Optional
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}
