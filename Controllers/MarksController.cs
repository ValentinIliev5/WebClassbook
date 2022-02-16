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
        public async Task<IActionResult> Index(string searchString)
        {

            var applicationDbContext = _context.Marks.Include(w => w.Student).
                ThenInclude(w => w.ApplicationUser).
                Include(m => m.Subject).
                Include(m => m.Teacher).
                ThenInclude(w => w.ApplicationUser);
            if (User.IsInRole("Teacher"))
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    return View(await applicationDbContext.
                        Where(w => w.TeacherID == GetCurrentTeacher().Id).
                        Where(w => w.Student.ApplicationUser.Name.Contains(searchString)).ToListAsync());
                }
                return View(await applicationDbContext.Where(w=>w.TeacherID==GetCurrentTeacher().Id).ToListAsync());
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                return View(await applicationDbContext.
                    Where(w => w.Student.ApplicationUser.Name.Contains(searchString)).ToListAsync());
            }
            return View(await applicationDbContext.ToListAsync());
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
        

        public string GetDescr(double number) 
        {
            if (number<3)
            {
                return "Bad";
            }
            else
            {
                switch (Math.Round(number))
                {
                    case 3:
                        return "Poor";
                    case 4:
                        return "Fair";
                    case 5:
                        return "Good";
                    case 6:
                        return "Excellent";
                }
            }
            return "";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,DateTime Date, double Number,string studentName)
        {
            Mark mark = new Mark();
            mark.Date = Date;
            mark.Number = Number;
            if (Number < 3)
            {
                mark.Number = 2;
            }
            mark.Description = GetDescr(Number);
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

            var mark = await _context.Marks.Include(w=>w.Subject).FirstAsync(w=>w.MarkID==id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["SubjectName"] = new SelectList(_context.Subject.Where(w=>w.Teachers.Select(w=>w.Id).Contains(mark.TeacherID)), "SubjectName", "SubjectName", mark.Subject.SubjectName);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime Date, double Number)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Marks.First(w => w.MarkID == id).Date = Date;

                    if (Number < 3)
                    {
                        _context.Marks.First(w => w.MarkID == id).Number = 2;
                    }

                    else _context.Marks.First(w => w.MarkID == id).Number = Number;
                    _context.Marks.First(w => w.MarkID == id).Description = GetDescr(Number);

                    if (_context.Subject.Any(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()))
                    {
                        _context.Marks.First(w => w.MarkID == id).SubjectID = _context.Subject.First(w => w.SubjectName == Request.Form["Subject.SubjectName"].ToString()).SubjectID;
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

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.
                Include(m=>m.Student).
                ThenInclude(m=>m.ApplicationUser).
                Include(m => m.Subject).
                Include(m => m.Teacher).
                ThenInclude(m=>m.ApplicationUser).
                FirstOrDefaultAsync(m => m.MarkID == id);
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
