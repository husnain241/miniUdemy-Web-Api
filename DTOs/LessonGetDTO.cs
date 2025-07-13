namespace MiniUdemyApi.DTOs
{
    public class LessonGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string Content { get; set; }     // ✅ Add this

        public int CourseId { get; set; }

        public string CourseTitle { get; set; }
    }
}
