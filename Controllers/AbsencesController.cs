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
    public class AbsencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public AbsencesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Absences
        public async Task<IActionResult> Index(string searchString, string sortList)
        {
            var applicationDbContext = _context.Absences.Include(w => w.Student).
                ThenInclude(w => w.ApplicationUser).
                Include(m => m.Subject).
                Include(m => m.Teacher).
                ThenInclude(w => w.ApplicationUser);

            bool? isPardoned;
            switch (sortList)
            {
                case "Pardoned":
                    isPardoned = true;
                    break;
                case "Not Pardoned":
                    isPardoned = false;
                    break;
                default:
                    isPardoned = null;
                    break;
            }

            if (!isPardoned.HasValue)
            {
                if (User.IsInRole("Teacher"))
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        return View(await applicationDbContext.
                            Where(w => w.TeacherID == GetCurrentTeacher().Id).
                            Where(w => w.Student.ApplicationUser.Name.Contains(searchString)).ToListAsync());
                    }
                    return View(await applicationDbContext.Where(w => w.TeacherID == GetCurrentTeacher().Id).ToListAsync());
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    return View(await applicationDbContext.
                        Where(w => w.Student.ApplicationUser.Name.Contains(searchString)).ToListAsync());
                }
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                if (User.IsInRole("Teacher"))
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        return View(await applicationDbContext.
                            Where(w => w.TeacherID == GetCurrentTeacher().Id).
                            Where(w => w.Student.ApplicationUser.Name.Contains(searchString)).
                            Where(w => w.Pardoned == isPardoned).ToListAsync());
                    }
                    return View(await applicationDbContext.Where(w => w.TeacherID == GetCurrentTeacher().Id).
                        Where(w => w.Pardoned == isPardoned).ToListAsync());
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    return View(await applicationDbContext.
                        Where(w => w.Student.ApplicationUser.Name.Contains(searchString)).
                        Where(w => w.Pardoned == isPardoned).ToListAsync());
                }
                return View(await applicationDbContext.
                    Where(w => w.Pardoned == isPardoned).ToListAsync());
            }
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> StudentsList(string searchString)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.Students.Include(m => m.ApplicationUser).Where(w => w.Grade.Contains(searchString)).ToListAsync());
            }
            return View(await _context.Students.Include(m => m.ApplicationUser).ToListAsync());
        }

        // GET: Absences/Create
        public IActionResult Create()
        {
            ViewData["SubjectName"] = new SelectList(_context.Subject.Include(w => w.Teachers).Where(
                w => w.Teachers.Contains(GetCurrentTeacher())), "SubjectName", "SubjectName");

            return View();
        }

        // POST: Absences/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, DateTime Date)
        {
            Absence absence = new Absence();
            absence.Date = Date;
            absence.Pardoned = false;
            if (ModelState.IsValid)
            {
                absence.StudentID = id;
                absence.TeacherID = GetCurrentTeacher().Id;
                absence.SubjectID = _context.Subject.First(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()).SubjectID;
                _context.Add(absence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectName"] = new SelectList(_context.Subject.Include(w => w.Teachers).Where(
                w => w.Teachers.Contains(_context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User))))
                , "SubjectName", "SubjectName");
            return View(absence);
        }

        // GET: Absences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absences.Include(w => w.Subject).FirstAsync(w => w.ID == id);
            if (absence == null)
            {
                return NotFound();
            }
            ViewData["SubjectName"] = new SelectList(_context.Subject.Where(w => w.Teachers.Select(w => w.Id).Contains(absence.TeacherID)), "SubjectName", "SubjectName", absence.Subject.SubjectName);
            return View(absence);
        }

        // POST: Absences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime Date, bool Pardoned)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Absences.Find(id).Date = Date;
                    _context.Absences.Find(id).Pardoned = Pardoned;
                    if (_context.Subject.Any(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()))
                    {
                        _context.Absences.Find(id).SubjectID = _context.Subject.First(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()).SubjectID;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectName"] = new SelectList(_context.Subject, "SubjectName", "SubjectName");
            return View();
        }

        // GET: Absences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absences.
                Include(m => m.Student).
                ThenInclude(m => m.ApplicationUser).
                Include(m => m.Subject).
                Include(m => m.Teacher).
                ThenInclude(m => m.ApplicationUser).
                FirstOrDefaultAsync(m => m.ID == id);

            if (absence == null)
            {
                return NotFound();
            }

            return View(absence);
        }

        // POST: Absences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var absence = await _context.Absences.FindAsync(id);
            _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbsenceExists(int id)
        {
            return _context.Absences.Any(e => e.ID == id);
        }

        public Teacher GetCurrentTeacher()
        {
            return _context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User));
        }
    }
}
