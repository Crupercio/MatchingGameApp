using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchingGameApp.Data;
using MatchingGameApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
            var items = await _context.GameItems.ToListAsync(); // Fetch all items into memory

            // Shuffle the items in memory
            var randomItems = items
                .OrderBy(_ => Guid.NewGuid()) // Shuffle in-memory
                .Take(4) // Take the desired number of items
                .ToList();

            return View(randomItems); // Pass the shuffled items to the view
        }

    }
}


