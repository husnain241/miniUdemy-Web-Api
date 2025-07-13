using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniUdemy.DTOs;
using MiniUdemy.Models;
using MiniUdemyApi.DTOs;
using MiniUdemyApi.Repositories;
using System.Security.Claims;

namespace MiniUdemyApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _lessonRepo;
        private readonly ICourseRepository _courseRepo;

        public LessonController(ILessonRepository lessonRepo, ICourseRepository courseRepo)
        {
            _lessonRepo = lessonRepo;
            _courseRepo = courseRepo;
        }

        // ✅ Add lesson to own course
        [HttpPost]
        public async Task<IActionResult> AddLesson([FromBody] LessonCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var course = await _courseRepo.GetByIdAsync(dto.CourseId);

            if (course == null) return NotFound("Course not found");
            if (course.CreatorId != userId) return StatusCode(403, "You can only add lessons to your own courses.");

            var lesson = new Lesson
            {
                Title = dto.Title,
                VideoUrl = dto.VideoUrl,
                Content = dto.Content     ,  

                CourseId = dto.CourseId
            };

            await _lessonRepo.AddAsync(lesson);
            await _lessonRepo.SaveAsync();

            return Ok(lesson);
        }

        // ✅ Get all lessons by courseId
        [HttpGet("{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonsByCourse(int courseId)
        {
            var lessons = await _lessonRepo.GetLessonsByCourseIdAsync(courseId);

            var result = lessons.Select(l => new LessonGetDTO
            {
                Id = l.Id,
                Title = l.Title,
                VideoUrl = l.VideoUrl,
                CourseId = l.CourseId,
                Content = l.Content,   // ✅ Add this line

                CourseTitle = l.Course?.Title
            });

            return Ok(result);
        }

        // ✅ Update a lesson (Teacher only)
        [HttpPut("{lessonId}")]
        public async Task<IActionResult> UpdateLesson(int lessonId, [FromBody] LessonUpdateDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var lesson = await _lessonRepo.GetByIdAsync(lessonId);
            if (lesson == null) return NotFound("Lesson not found");

            var course = await _courseRepo.GetByIdAsync(lesson.CourseId);
            if (course == null || course.CreatorId != userId)
                return StatusCode(403, "You can only update lessons from your own courses.");

            lesson.Title = dto.Title;
            lesson.VideoUrl = dto.VideoUrl;
            lesson.Content = dto.Content;

            _lessonRepo.Update(lesson);
            await _lessonRepo.SaveAsync();

            return Ok(new { message = "Lesson updated successfully." });
        }

        // ✅ Delete a lesson (Teacher only)
        [HttpDelete("{lessonId}")]
        public async Task<IActionResult> DeleteLesson(int lessonId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var lesson = await _lessonRepo.GetByIdAsync(lessonId);
            if (lesson == null) return NotFound("Lesson not found");

            var course = await _courseRepo.GetByIdAsync(lesson.CourseId);
            if (course == null || course.CreatorId != userId)
                return StatusCode(403, "You can only delete your own lessons ");  // ✅ custom message allowed here

            _lessonRepo.Delete(lesson);
            await _lessonRepo.SaveAsync();

            return Ok(new { message = "Lesson deleted successfully." });
        }

    }
}
