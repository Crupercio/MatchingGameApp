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
        public async Task<IActionResult> Match(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return RedirectToAction("SelectCategory");
            }

            ViewData["Category"] = category;

            // Fetch unique items for the selected category
            var items = await _context.GameItems
                .Where(g => g.Category == category)
                .GroupBy(g => g.Id)
                .Select(g => g.First())
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

        public async Task<IActionResult> Leaderboard()
        {
            var scores = await _context.Scores
                .Include(s => s.User)
                .OrderByDescending(s => s.Points)
                .ToListAsync();

            return View(scores);
        }
    }
}
