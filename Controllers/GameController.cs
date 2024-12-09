using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchingGameApp.Data;
using MatchingGameApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MatchingGameApp.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Game/Match
        public async Task<IActionResult> Match()
        {
            // Fetch all items from the database
            var items = await _context.GameItems
                .ToListAsync();

            if (!items.Any())
            {
                return NotFound("No game items available.");
            }

            // Select a random category
            var categories = items.Select(i => i.Category).Distinct().ToList();
            var randomCategory = categories[new Random().Next(categories.Count)];

            // Filter items by the selected category
            var categoryItems = items
                .Where(i => i.Category == randomCategory)
                .OrderBy(_ => Guid.NewGuid()) // Shuffle in memory
                .Take(8) // Take 8 unique items
                .ToList();

            ViewData["Category"] = randomCategory;
            return View(categoryItems);
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


        public async Task<IActionResult> Leaderboard()
        {
            // Fetch all scores including user data
            var scores = await _context.Scores
                .Include(s => s.User) // Include the related user data
                .OrderByDescending(s => s.Points) // Order by score in descending order
                .ToListAsync();

            return View(scores);
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomCategory()
        {
            // Fetch all unique categories
            var categories = await _context.GameItems
                .Select(g => g.Category)
                .Distinct()
                .ToListAsync();

            if (!categories.Any())
            {
                return NotFound("No categories available.");
            }

            // Select a random category
            var randomCategory = categories[new Random().Next(categories.Count)];

            // Fetch items from the selected category
            var items = await _context.GameItems
                .Where(g => g.Category == randomCategory)
                .ToListAsync();

            return Json(new { category = randomCategory, items });
        }



        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var scores = await _context.Scores
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.DateAchieved)
                .ToListAsync();

            return View(scores);
        }




    }
}


