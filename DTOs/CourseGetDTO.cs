namespace MiniUdemyApi.DTOs
{
    public class CourseGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }
    }
}
