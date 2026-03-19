using CS_maturita_test.Data;
using CS_maturita_test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CS_maturita_test.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public NotesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var notes = await _dbContext.Notes
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Id)
                .ToListAsync();

            return View(notes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["NoteError"] = "Poznámka nesmí být prázdná.";
                return RedirectToAction(nameof(Index));
            }

            var note = new Note
            {
                Content = content.Trim(),
                UserId = userId
            };

            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var note = await _dbContext.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note is null)
            {
                return NotFound();
            }

            _dbContext.Notes.Remove(note);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
