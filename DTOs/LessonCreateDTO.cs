namespace MiniUdemyApi.DTOs
{
    public class LessonCreateDTO
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string Content { get; set; }     // ✅ Add this

        public int CourseId { get; set; }
    }
}
