using MiniUdemy.Models;
using MiniUdemy.Repositories;
using MiniUdemy.Models;

namespace MiniUdemyApi.Repositories
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId);

    }
}
