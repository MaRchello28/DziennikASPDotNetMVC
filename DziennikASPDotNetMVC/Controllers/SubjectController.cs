using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    public class SubjectController : Controller
    {
        private readonly MyDbContext db;
        public SubjectController(MyDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            // Pobranie listy użytkowników z bazy danych
            var subjects = await db.Subjects.ToListAsync();

            // Przekazanie danych do widoku
            return View(subjects);
        }

        // GET: SubjectController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Pobranie użytkownika z bazy danych
            var subject = await db.Subjects.FirstOrDefaultAsync(u => u.subjectId == id);

            // Sprawdzenie, czy użytkownik istnieje
            if (subject == null)
            {
                return NotFound(); // Zwrot kodu 404, jeśli nie znaleziono użytkownika
            }

            // Przekazanie użytkownika do widoku
            return View(subject);
        }

        // GET: SubjectController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubjectController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: SubjectController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var subject = await db.Subjects.FirstOrDefaultAsync(u => u.subjectId == id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: SubjectController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("subjectId,name")] Subject subject)
        {
            if (id != subject.subjectId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(subject);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!SubjectExists(subject.subjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(subject);
        }

        // GET: SubjectController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await db.Subjects.FirstOrDefaultAsync(s => s.subjectId == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: SubjectController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await db.Subjects.FirstOrDefaultAsync(u => u.subjectId == id);

            if (subject != null)
            {
                db.Subjects.Remove(subject);
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private bool SubjectExists(int id)
        {
            return db.Subjects.Any(e => e.subjectId == id);
        }
    }
}
