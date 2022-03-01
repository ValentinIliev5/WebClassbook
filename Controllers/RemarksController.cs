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
    public class RemarksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        const int ITEMS_PER_PAGE = 5;

        public RemarksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString,int currentPage=1)
        {
            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE.ToString();
            var applicationDbContext = _context.Remarks.
                Include(w => w.Student).ThenInclude(w => w.ApplicationUser).
                Include(w=>w.Teacher).ThenInclude(w=>w.ApplicationUser);
            if (User.IsInRole("Teacher"))
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchModel = applicationDbContext.
                        Where(w => w.TeacherID == GetCurrentTeacher().Id).
                        Where(w => w.Student.ApplicationUser.Name.Contains(searchString));

                    ViewData["ItemsCount"] = searchModel.Count().ToString();

                    return View(await searchModel.Skip((currentPage - 1)
                               * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
                }

                var teacherModel = applicationDbContext.
                            Where(w => w.TeacherID == GetCurrentTeacher().Id);
                ViewData["ItemsCount"] = teacherModel.Count().ToString();


                return View(await teacherModel.Skip((currentPage - 1)
                    * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchModel = applicationDbContext.
                    Where(w => w.Student.ApplicationUser.Name.Contains(searchString));
                ViewData["ItemsCount"] = searchModel.Count().ToString();
                return View(await searchModel.Skip((currentPage - 1)
                    * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
            }

            ViewData["ItemsCount"] = applicationDbContext.Count().ToString();

            return View(await applicationDbContext.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
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

        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(int id, DateTime Date, bool IsPositive,string Description)
        {
            Remark remark = new Remark();
            remark.Date = Date;
            remark.IsPositive = IsPositive;
            remark.Description = Description;
            if (ModelState.IsValid)
            {
                remark.StudentID = id;
                remark.TeacherID = GetCurrentTeacher().Id;
                _context.Add(remark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(remark);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remark = await _context.Remarks.FirstAsync(w => w.ID == id);
            if (remark == null)
            {
                return NotFound();
            }

            return View(remark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime Date, bool IsPositive,string Description)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Remarks.Find(id).Date = Date;
                    _context.Remarks.Find(id).IsPositive = IsPositive;
                    _context.Remarks.Find(id).Description = Description;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remark = await _context.Remarks.
                Include(m => m.Student).
                ThenInclude(m => m.ApplicationUser).
                Include(m => m.Teacher).
                ThenInclude(m => m.ApplicationUser).
                FirstOrDefaultAsync(m => m.ID == id);

            if (remark == null)
            {
                return NotFound();
            }

            return View(remark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var remark = await _context.Remarks.FindAsync(id);
            _context.Remarks.Remove(remark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RemarkExists(int id)
        {
            return _context.Remarks.Any(e => e.ID == id);
        }

        public Teacher GetCurrentTeacher()
        {
            return _context.
                Teachers.First(w => w.ApplicationUserID == _userManager.GetUserId(HttpContext.User));
        }
    }
}
