using MiniUdemy.Models;

namespace MiniUdemy.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }

        // Foreign Key (Creator)
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        // Relations
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
