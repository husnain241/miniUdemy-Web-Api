using Microsoft.EntityFrameworkCore;
using MiniUdemy.Data;
using MiniUdemy.Models;
using MiniUdemy.Repositories;


namespace MiniUdemyApi.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCoursesWithCreatorAsync()
        {
            return await _context.Courses.Include(c => c.Creator).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherAsync(string teacherId)
        {
            return await _context.Courses
                .Where(c => c.CreatorId.ToString() == teacherId)
                .Include(c => c.Creator)
                .ToListAsync();
        }


        public async Task<Course> GetCourseWithLessonsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }
    }
}
