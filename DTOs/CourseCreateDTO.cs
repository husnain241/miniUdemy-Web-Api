namespace MiniUdemyApi.DTOs
{
    public class CourseCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
    }

}
