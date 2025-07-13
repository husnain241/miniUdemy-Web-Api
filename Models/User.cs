using Microsoft.AspNetCore.Identity;
using MiniUdemy.Models;

namespace MiniUdemy.Models
{
    public class User : IdentityUser<int> // 👈 Inherit from IdentityUser with int primary key
    {

        public string FullName { get; set; }

        public string Role { get; set; }  // Student or Teacher


        //public int Id { get; set; }
        //public string Email { get; set; }
        //public string PasswordHash { get; set; }
        //public string Role { get; set; } = "Student"; // Student or Teacher

        // Relations
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Course> CoursesCreated { get; set; }
    }
}
