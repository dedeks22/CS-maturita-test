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
        public async Task<IActionResult> Index(bool onlyImportant = false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var query = _dbContext.Notes.Where(n => n.UserId == userId);

            if (onlyImportant)
            {
                query = query.Where(n => n.IsImportant);
            }

            var notes = await query
                .OrderByDescending(n => n.CreatedAt)
                .ThenByDescending(n => n.Id)
                .ToListAsync();

            ViewData["OnlyImportant"] = onlyImportant;
            return View(notes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string title, string content, bool isImportant)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                TempData["NoteError"] = "Nadpis poznámky nesmí být prázdný.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["NoteError"] = "Poznámka nesmí být prázdná.";
                return RedirectToAction(nameof(Index));
            }

            var note = new Note
            {
                Title = title.Trim(),
                Content = content.Trim(),
                UserId = userId,
                IsImportant = isImportant,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleImportant(int id, bool onlyImportant = false)
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

            note.IsImportant = !note.IsImportant;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { onlyImportant });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, bool onlyImportant = false)
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

            return RedirectToAction(nameof(Index), new { onlyImportant });
        }
    }
}
