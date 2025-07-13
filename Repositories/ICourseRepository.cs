using MiniUdemy.Models;
using MiniUdemy.Repositories;

namespace MiniUdemyApi.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesWithCreatorAsync();

        Task<IEnumerable<Course>> GetCoursesByTeacherAsync(string teacherId);

        Task<Course> GetCourseWithLessonsAsync(int courseId);

    }
}
