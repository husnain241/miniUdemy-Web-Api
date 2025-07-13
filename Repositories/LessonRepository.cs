using Microsoft.EntityFrameworkCore;
using MiniUdemy.Data;
using MiniUdemy.Models;
using MiniUdemy.Repositories;
using MiniUdemy.Data;
using MiniUdemy.Models;

namespace MiniUdemyApi.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId)
        {
            return await _context.Lessons.Where(l => l.CourseId == courseId).ToListAsync();
        }
    }
}
