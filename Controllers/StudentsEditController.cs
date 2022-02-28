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
    public class StudentsEditController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public StudentsEditController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
                if (roles.Any(w => w == "Student"))
                {
                    forReturn.Add(user);
                }
            }

            return View(forReturn);
        }

        public async Task<IActionResult> EditStudent(string id)
        {

            if (id == string.Empty)
            {
                return NotFound();
            }

            var student = await _context.Users.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }


            return View(_context.Students.First(w => w.ApplicationUserID == student.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(string id, string grade)
        {
            Student student = _context.Students.
                First(w => w.ApplicationUserID == id);

            if (ModelState.IsValid)
            {
                

                try
                {

                    if (grade != student.Grade)
                    {
                        student.Grade = grade;
                        if (_context.Students.Where(z => z.Grade == grade).Count() != 0)
                        {
                            student.ClassNumber = _context.Students.Where(w => w.Grade == student.Grade).Max(w => w.ClassNumber) + 1;
                        }
                        else student.ClassNumber = 1;
                        _context.Update(student);
                        await _context.SaveChangesAsync();

                    }
                    
                    _context.Users.First(w => w.Id == id).Name = Request.Form["ApplicationUser.Name"];
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            Student student = _context.Students.
                First(w => w.ApplicationUserID == id);

            if (ModelState.IsValid)
            {


                try
                {
                    _context.Students.Remove(student);
                    await _userManager.DeleteAsync(_context.Users.First(w => w.Id == id));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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


            return View(student);
        }



        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }


    }
}
