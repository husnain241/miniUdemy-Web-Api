using Microsoft.EntityFrameworkCore;
using MiniUdemy.Data;
using MiniUdemy.Models;
using MiniUdemy.Repositories;


namespace MiniUdemyApi.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(int userId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsForCourseWithUsersAsync(int courseId)
        {
            return await _context.Enrollments
                .Include(e => e.User)           // 👈 Get student info
                .Include(e => e.Course)         // 👈 Optional: useful for showing course title
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }
    }
}
