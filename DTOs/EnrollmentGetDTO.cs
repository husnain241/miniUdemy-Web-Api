namespace MiniUdemy.DTOs
{
    public class EnrollmentGetDTO
    {
        public int Id { get; set; }
        public string StudentEmail { get; set; }
        public DateTime EnrollDate { get; set; }
        public string CourseTitle { get; set; }
    }

}
