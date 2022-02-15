using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebClassbook.Data;
using WebClassbook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebClassbook.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class MarksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> StudentsList(string searchString) 
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.Students.Include(m => m.ApplicationUser).Where(w=>w.Grade.Contains(searchString)).ToListAsync());
            }
            return View(await _context.Students.Include(m=>m.ApplicationUser).ToListAsync());
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Marks.Include(w=>w.Student).
                ThenInclude(w=>w.ApplicationUser).
                Include(m => m.Subject).
                Include(m => m.Teacher).
                ThenInclude(w=>w.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Subject)
                .Include(m => m.Teacher)
                .FirstOrDefaultAsync(m => m.MarkID == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create()
        {
            ViewData["SubjectName"] = new SelectList(_context.Subject.Include(w=>w.Teachers).Where(
                w => w.Teachers.Contains(GetCurrentTeacher())), "SubjectName", "SubjectName");

            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,DateTime Date, double Number,string studentName)
        {
            Mark mark = new Mark();
            mark.Date = Date;
            mark.Number = Number;
            if (Number < 3)
            {
                mark.Description = "Слаб";
                mark.Number = 2;
            }
            else
            {
                switch (Math.Round(Number))
                {
                    case 3:
                        mark.Description = "Среден";
                        break;
                    case 4:
                        mark.Description = "Добър";
                        break;
                    case 5:
                        mark.Description = "Мн. Добър";
                        break;
                    case 6:
                        mark.Description = "Отличен";
                        break;
                }
            }
            mark.StudentID = id;
            mark.TeacherID = GetCurrentTeacher().Id;
            mark.SubjectID = _context.Subject.First(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()).SubjectID;
            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectName"] = new SelectList(_context.Subject.Include(w => w.Teachers).Where(
                w => w.Teachers.Contains(_context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User))))
                , "SubjectName", "SubjectName");

            return View(mark);
        }
        public Teacher GetCurrentTeacher() {
            return _context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User));
        }
        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", mark.SubjectID);
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "Id", "Id", mark.TeacherID);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarkID,Date,Number,Description,StudentID,SubjectID,TeacherID")] Mark mark)
        {
            if (id != mark.MarkID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.MarkID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", mark.SubjectID);
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "Id", "Id", mark.TeacherID);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Subject)
                .Include(m => m.Teacher)
                .FirstOrDefaultAsync(m => m.MarkID == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mark = await _context.Marks.FindAsync(id);
            _context.Marks.Remove(mark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
            return _context.Marks.Any(e => e.MarkID == id);
        }
    }
}
