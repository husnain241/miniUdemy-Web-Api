using MiniUdemy.Models;
using MiniUdemy.Repositories;

namespace MiniUdemyApi.Repositories
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(int userId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsForCourseWithUsersAsync(int courseId);

        Task<Enrollment> GetByIdAsync(int id);
        void Delete(Enrollment entity);
        Task SaveAsync();

    }
}
