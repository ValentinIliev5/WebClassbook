using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WebClassbook.Data;
using WebClassbook.Models;


namespace WebClassbook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherEditController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherEditController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            List<ApplicationUser> forReturn = new List<ApplicationUser>();


            foreach (ApplicationUser user in _context.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(w=>w=="Teacher"))
                {
                    forReturn.Add(user);
                }
            }

            return View(forReturn);
        }

        public async Task<IActionResult> EditTeacher(string id)
        {

            if (id == string.Empty)
            {
                return NotFound();
            }

            var teacher = await _context.Users.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            ViewData["SubjectName"]= new SelectList(_context.Subject, "SubjectName", "SubjectName");
            
            return View(_context.Teachers.First(w=>w.ApplicationUserID==teacher.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(string id, string grade)
        {
            Teacher teacher = _context.Teachers.
                First(w => w.ApplicationUserID == id);

            if (ModelState.IsValid)
            { 
                
                List<string> subjectnames = Request.Form["Subjects"].ToList();
                List<Subject> subjectsToAdd = new List<Subject>();
                foreach (var item in subjectnames)
                {
                    subjectsToAdd.Add(_context.Subject.First(w => w.SubjectName == item));
                }
                
                teacher.Grade = grade;
                

                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();


                    _context.Users.First(w => w.Id == id).Name = Request.Form["ApplicationUser.Name"];
                    await _context.SaveChangesAsync();


                    _context.Teachers.Include(w => w.Subjects).
                    First(w => w.ApplicationUserID == id).Subjects.Clear();

                    foreach (var item in subjectsToAdd)
                    {
                        _context.Teachers.Include(w => w.Subjects).First(w => w.Id == teacher.Id).Subjects.
                            Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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

            
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeacher(string id)
        {
            Teacher teacher = _context.Teachers.
                First(w => w.ApplicationUserID == id);

            if (ModelState.IsValid)
            {

                
                try
                {
                    _context.Teachers.Remove(teacher);
                    await _userManager.DeleteAsync(_context.Users.First(w=>w.Id==id));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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

            return View(teacher);
        }



        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }


    }
}
