using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniUdemy.Data;
using MiniUdemy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MiniUdemyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize(Roles= "Admin")] // Only Admin can use this controller
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }

        [HttpGet("pending-teachers")]
        public async Task<IActionResult> GetPendingTeachers()
        {
            var requests = await _context.TeacherRequests.ToListAsync();
            return Ok(requests);
        }

        [HttpPost("approve-teacher/{id}")]
        public async Task<IActionResult> ApproveTeacher(int id)
        {
            var request = await _context.TeacherRequests.FindAsync(id);
            if (request == null)
                return NotFound("Teacher request not found.");

            // 1. Try to find user by email
            var user = await _userManager.FindByEmailAsync(request.Email);

            // 2. If not found, create the user first
            if (user == null)
            {
                user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    UserName = request.Email, // UserName usually same as email
                     Role = "Teacher"  // 👈 Add this line!

                };

                var createResult = await _userManager.CreateAsync(user, "DefaultPassword@123");

                if (!createResult.Succeeded)
                    return BadRequest("Failed to create user: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }

            // 3. Add "Teacher" role
            var roleResult = await _userManager.AddToRoleAsync(user, "Teacher");
            if (!roleResult.Succeeded)
                return BadRequest("Failed to assign role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            // 4. Remove the request
            _context.TeacherRequests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok("Teacher approved and user created successfully.");
        }


        [HttpDelete("reject-teacher/{id}")]
        public async Task<IActionResult> RejectTeacher(int id)
        {
            var request = await _context.TeacherRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            _context.TeacherRequests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok("Teacher request rejected successfully.");
        }
    }
}
