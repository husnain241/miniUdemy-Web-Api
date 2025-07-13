using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniUdemy.DTOs;
using MiniUdemy.Models;
using MiniUdemyApi.Repositories;
using System.Security.Claims;

namespace MiniUdemyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollRepo;
        private readonly ICourseRepository _courseRepo;

        public EnrollmentController(IEnrollmentRepository enrollRepo, ICourseRepository courseRepo)
        {
            _enrollRepo = enrollRepo;
            _courseRepo = courseRepo;
            
        }

        // ✅ STUDENT: Enroll in a course
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var enrollment = new Enrollment
            {
                CourseId = dto.CourseId,
                UserId = userId,
                EnrolledAt = DateTime.UtcNow
            };

            await _enrollRepo.AddAsync(enrollment);
            await _enrollRepo.SaveAsync();

            return Ok("Enrolled successfully.");
        }

        // ✅ STUDENT: View own enrollments
        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enrollments = await _enrollRepo.GetEnrollmentsByUserIdAsync(userId);
                
            var result = enrollments.Select(e => new
            {
                e.Id,
                CourseTitle = e.Course.Title,
                e.EnrolledAt
            });

            return Ok(result);
        }

        // ✅ TEACHER: View students enrolled in a course
        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetEnrollmentsForCourse(int courseId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var course = await _courseRepo.GetByIdAsync(courseId);

            if (course == null) return NotFound("Course not found.");
            if (course.CreatorId != userId) return StatusCode(403, "This is not your course.");  // ✅ custom message allowed here


            var enrollments = await _enrollRepo.GetEnrollmentsForCourseWithUsersAsync(courseId);

            var result = enrollments.Select(e => new EnrollmentGetDTO
            {
                Id = e.Id,
                StudentEmail = e.User?.Email,
                EnrollDate = e.EnrolledAt,
                CourseTitle = course.Title
            });

            return Ok(result);
        }


        // ✅ STUDENT: Cancel (delete) enrollment
        [HttpDelete("{enrollmentId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CancelEnrollment(int enrollmentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enrollment = await _enrollRepo.GetByIdAsync(enrollmentId);

            if (enrollment == null)
                return NotFound("Enrollment not found.");

            if (enrollment.UserId != userId)
                return StatusCode(403, "You can only cancel your own enrollments.");

            _enrollRepo.Delete(enrollment);
            await _enrollRepo.SaveAsync();

            return Ok("Enrollment cancelled successfully.");
        }





    }
}
