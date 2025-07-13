using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniUdemy.Models;
using MiniUdemyApi.DTOs;
using MiniUdemyApi.Repositories;
using System.Security.Claims;

namespace MiniUdemyApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepo;

        public CourseController(ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }

        // ✅ Public: anyone can see all courses
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseRepo.GetCoursesWithCreatorAsync();

            var result = courses.Select(c => new CourseGetDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                ThumbnailUrl = c.ThumbnailUrl,
                CreatorId = c.CreatorId,
                CreatorName = c.Creator?.UserName
            });

            return Ok(result);
        }

        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token!");

            var courses = await _courseRepo.GetCoursesByTeacherAsync(userId);

            var result = courses.Select(c => new CourseGetDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                ThumbnailUrl = c.ThumbnailUrl,
                CreatorId = c.CreatorId,
                CreatorName = c.Creator?.UserName
            });

            return Ok(result);
        }


        // ✅ Teacher: create new course
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDTO courseDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token!");

            var course = new Course
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                Price = courseDto.Price,
                ThumbnailUrl = courseDto.ThumbnailUrl,
                CreatorId = int.Parse(userId)
            };

            await _courseRepo.AddAsync(course);
            await _courseRepo.SaveAsync();

            return Ok(course);
        }

        // ✅ Teacher: update own course
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var course = await _courseRepo.GetByIdAsync(id);

            if (course == null) return NotFound();
            if (course.CreatorId != userId)
                return Forbid("You can only edit your own course.");

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Price = dto.Price;
            course.ThumbnailUrl = dto.ThumbnailUrl;

            _courseRepo.Update(course);
            await _courseRepo.SaveAsync();

            return Ok("Course updated");
        }

        // ✅ Teacher: delete own course
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)   
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var course = await _courseRepo.GetByIdAsync(id);

            if (course == null) return NotFound();
            if (course.CreatorId != userId)
                return Forbid("You can only delete your own course.");

            _courseRepo.Delete(course);
            await _courseRepo.SaveAsync();

            return Ok("Course deleted");
        }
    }
}
