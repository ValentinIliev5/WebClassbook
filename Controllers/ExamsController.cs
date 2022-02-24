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
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExamsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Teacher GetCurrentTeacher()
        {
            return _context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User));
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = _context.Exams
                .Include(w => w.Teacher)
                .ThenInclude(w => w.ApplicationUser)
                .Include(w => w.Subject);
            if (User.IsInRole("Teacher"))
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return View(await applicationDbContext.
                        Where(w => w.TeacherID == GetCurrentTeacher().Id).
                        Where(w => w.Subject.SubjectName.Contains(searchString)).ToListAsync());
                }
                return View(await applicationDbContext.Where(w=>w.TeacherID==GetCurrentTeacher().Id).ToListAsync());
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                return View(await applicationDbContext.
                    Where(w => w.Subject.SubjectName.Contains(searchString)).ToListAsync());
            }
            return View(await applicationDbContext.ToListAsync());
        }

        //GET 
        public IActionResult Create()
        {
            ViewData["SubjectName"] = new SelectList(_context.Subject.Include(w => w.Teachers).Where(
                w => w.Teachers.Contains(GetCurrentTeacher())), "SubjectName", "SubjectName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime Date, string Class, string Description)
        {
            Exam exam = new Exam();
            exam.Date = Date;
            exam.Class = Class;
            exam.Description = Description;
            exam.TeacherID = GetCurrentTeacher().Id;
            exam.SubjectID = _context.Subject.First(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()).SubjectID;
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectName"] = new SelectList(_context.Subject.Include(w => w.Teachers).Where(
                w => w.Teachers.Contains(_context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User))))
                , "SubjectName", "SubjectName");

            return View(exam);
        }
        //Get
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.Include(w => w.Subject).FirstAsync(w => w.ID == id);
            if (exam == null)
            {
                return NotFound();
            }
            ViewData["SubjectName"] = new SelectList(_context.Subject.Where(w => w.Teachers.Select(w => w.Id).Contains(exam.TeacherID)), "SubjectName", "SubjectName", exam.Subject.SubjectName);
            return View(exam);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime Date, string Class, string Description)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Exams.First(w => w.ID == id).Date = Date;
                    _context.Exams.First(w => w.ID == id).Class = Class;
                    _context.Exams.First(w => w.ID == id).Description = Description;
                    if (_context.Subject.Any(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()))
                    {
                        _context.Exams.First(w => w.ID == id).SubjectID =
                            _context.Subject.First(w => w.
                            SubjectName == Request.Form["Subject.SubjectName"].ToString()).SubjectID;
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

        //GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Exam = await _context.Exams
                .Include(w => w.Teacher)
                .ThenInclude(w => w.ApplicationUser)
                .Include(w => w.Subject)
                .FirstOrDefaultAsync(w => w.ID == id);
            if (Exam == null)
            {
                return NotFound();
            }
            return View(Exam);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    
}
