using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchingGameApp.Data;
using MatchingGameApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MatchingGameApp.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public GameController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        private readonly UserManager<ApplicationUser> _userManager;
        // GET: /Game/Match
        public async Task<IActionResult> Match(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return RedirectToAction("SelectCategory");
            }

            ViewData["Category"] = category;

            // Ensure only unique items are fetched
            var items = await _context.GameItems
                .Where(g => g.Category == category)
                .GroupBy(g => g.Id) // Group by Id to ensure uniqueness
                .Select(g => g.First()) // Select the first item in each group
                .ToListAsync();

          

            return View(items);
        }




        [HttpPost]
        public async Task<IActionResult> SaveScore([FromBody] SaveScoreDto saveScoreDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data received.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(saveScoreDto.Category))
            {
                return BadRequest("Category is required.");
            }

            var existingScore = await _context.Scores
                .Where(s => s.UserId == userId && s.Category == saveScoreDto.Category && !saveScoreDto.IsFinal)
                .OrderByDescending(s => s.DateAchieved)
                .FirstOrDefaultAsync();

            if (existingScore != null)
            {
                existingScore.Points = saveScoreDto.Points;
                existingScore.DateAchieved = DateTime.UtcNow;
                _context.Scores.Update(existingScore);
            }
            else if (saveScoreDto.IsFinal)
            {
                var newScore = new Score
                {
                    UserId = userId,
                    Points = saveScoreDto.Points,
                    DateAchieved = DateTime.UtcNow,
                    Category = saveScoreDto.Category
                };
                _context.Scores.Add(newScore);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryItems(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category is required.");
            }

            var items = await _context.GameItems
                .Where(g => g.Category == category)
                .GroupBy(g => g.Id)
                .Select(g => g.First())
                .ToListAsync();

            return Json(new { category, items });
        }

        [HttpGet]
        public async Task<IActionResult> SelectCategory()
        {
            var categories = await _context.GameItems
                .Select(g => g.Category)
                .Distinct()
                .ToListAsync();

            if (!categories.Any())
            {
                return NotFound("No categories available.");
            }

            return View(categories);
        }



        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Set the default profile picture URL
            ViewData["ProfilePicture"] = user.ProfilePictureUrl ?? "/images/profile-default.png";
            ViewData["UserName"] = user.UserName;

            var scores = await _context.Scores
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.DateAchieved)
                .ToListAsync(); 

            return View(scores);
        }

        public async Task<IActionResult> Leaderboard()
        {
            var scores = await _context.Scores
                .Include(s => s.User) // Include User data
                .OrderByDescending(s => s.Points)
                .Select(s => new
                {
                    Rank = 0, // Rank calculation will happen later
                    Points = s.Points,
                    DateAchieved = s.DateAchieved,
                    Category = s.Category,
                    UserName = s.User != null && !string.IsNullOrEmpty(s.User.UserName)
                        ? s.User.UserName.Trim() : "Unknown",
                    ProfilePictureUrl = s.User != null && !string.IsNullOrEmpty(s.User.ProfilePictureUrl)
                        ? s.User.ProfilePictureUrl : "/images/profile-default.png"
                })
                .ToListAsync();

            return View(scores);
        }







        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(string profilePictureUrl)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.ProfilePictureUrl = profilePictureUrl;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update profile picture.");
                return RedirectToAction("Profile");
            }

            TempData["SuccessMessage"] = "Profile picture updated successfully!";
            return RedirectToAction("Profile");
        }

        public IActionResult UpdatePicture()
        {
            return View();
        }






    }
}
