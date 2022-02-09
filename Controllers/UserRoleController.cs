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
    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            List<ApplicationUser> forReturn = new List<ApplicationUser>();

            //forReturn = _context.Users.
            //    FromSqlRaw(@"SELECT * FROM 
            //                AspNetUsers U JOIN AspNetUserRoles UR
            //                ON U.Id = UR.UserId
            //                WHERE UR.RoleId = 4
            //                EXCEPT
            //                SELECT * FROM
            //                AspNetUsers U JOIN AspNetUserRoles UR
            //                ON U.Id = UR.UserId
            //                WHERE UR.RoleId = 1 OR UR.RoleId = 2 OR UR.RoleId = 3")
            //    .ToList();

            foreach (ApplicationUser user in _context.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count == 1 && roles.First() == "User")
                {
                    forReturn.Add(user);
                }
            }

            return View(forReturn);
        }

        public async Task<IActionResult> MakeTeacher(string id)
        {

            if (id == string.Empty)
            {
                return NotFound();
            }

            ViewData["SubjectName"] = new SelectList(_context.Subject, "SubjectName", "SubjectName");
            Teacher teacher = new Teacher();
            teacher.ApplicationUserID = id;
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeTeacher(string id, string Grade)
        {
            Teacher teacher = new Teacher();
            if (ModelState.IsValid)
            {

                teacher.Grade = Grade;
                teacher.ApplicationUserID = id;
                List<string> subjectnames = new List<string>();
                subjectnames = Request.Form["Subjects"].ToList();//RABoti!!!
                List<Subject> subjectsToAdd = new List<Subject>();
                foreach (var item in subjectnames)
                {
                    subjectsToAdd.Add(_context.Subject.First(w => w.SubjectName == item));

                }


                _context.Add(teacher);
                await _context.SaveChangesAsync();
                foreach (var item in subjectsToAdd)
                {
                    _context.Teachers.Include(w => w.Subjects).First(w => w.Id == teacher.Id).Subjects.
                        Add(item);
                }
                await _userManager.AddToRoleAsync(_context.Users.First(w => w.Id == id), "Teacher");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["SubjectName"] = new SelectList(_context.Subject, "SubjectName", "SubjectName");
            return View(teacher);
        }


        public async Task<IActionResult> MakeStudent(string id)
        {

            if (id == string.Empty)
            {
                return NotFound();
            }

            Student student = new Student();
            student.ApplicationUserID = id;
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeStudent(string id, string grade)
        {
            Student student = new Student();
            if (ModelState.IsValid)
            {
                student.Grade = grade;
                if (_context.Students.Where(w => w.Grade == grade).Count() > 0)
                {
                    student.ClassNumber = _context.Students.Where(w => w.Grade == grade)
                                                .Max(w => w.ClassNumber) + 1;
                }
                else student.ClassNumber = 1;

                student.ApplicationUserID = id;

                _context.Add(student);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(_context.Users.First(w => w.Id == id), "Student");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(student);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string id, string grade)
        {
            Admin admin = new Admin();

            _context.Add(admin);
            await _context.SaveChangesAsync();
            await _userManager.AddToRoleAsync(_context.Users.First(w => w.Id == id), "Admin");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    } 
    
}