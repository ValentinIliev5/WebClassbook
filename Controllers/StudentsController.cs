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
    [Authorize(Roles = "Student")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public string GetCurrentApplicationUserId() 
        {
            return _userManager.GetUserId(HttpContext.User);
        }

        public Student GetCurrentStudent() 
        {
            return _context.Students.Include(w=>w.ApplicationUser).First(w => w.ApplicationUserID == GetCurrentApplicationUserId());
        }


        public IActionResult Index()
        {
            string avgMark = _context.Marks.Where(w => w.StudentID == GetCurrentStudent().ID).Average(w => w.Number).ToString("0.00");
            ViewData["AvgMark"] = avgMark;

            string incExams = _context.Exams.Where(w=>w.Class==GetCurrentStudent().Grade).Count(w => w.Date > DateTime.Now).ToString();
            ViewData["incExams"] = incExams;

            string pardonedAbsences = _context.Absences.Where(w => w.StudentID == GetCurrentStudent().ID).Count(w => w.Pardoned).ToString();
            string unPardonedAbsences = _context.Absences.Where(w => w.StudentID == GetCurrentStudent().ID).Count(w => !w.Pardoned).ToString();
            ViewData["pardoned"] = pardonedAbsences;

            ViewData["unPardoned"] = unPardonedAbsences;

            ViewData["StudentInfo"]= GetCurrentStudent().ApplicationUser.Name + " - " + GetCurrentStudent().Grade;
            return View();

        }
        public async Task<IActionResult> MyMarks(string searchString)
        {
            var applicationDbContext = _context.Marks.Include(w => w.Student).
               ThenInclude(w => w.ApplicationUser).
               Include(m => m.Subject).
               Include(m => m.Teacher).
               ThenInclude(w => w.ApplicationUser);

            if (!string.IsNullOrEmpty(searchString))
            {
                return View( await applicationDbContext.
                    Where(w => w.StudentID == GetCurrentStudent().ID).
                    Where(w => w.Subject.SubjectName.Contains(searchString)).ToListAsync());
            }
            return View( await applicationDbContext.Where(w => w.StudentID == GetCurrentStudent().ID).ToListAsync());

        }
        public async Task<IActionResult> MyExams(string searchString) 
        {
            var applicationDbContext = _context.Exams.Include(w => w.Subject).
                Include(w=>w.Teacher).ThenInclude(w=>w.ApplicationUser);

            if (!string.IsNullOrEmpty(searchString))
            {
                return View(await applicationDbContext.
                    Where(w => w.Class == GetCurrentStudent().Grade).
                    Where(w => w.Subject.SubjectName.Contains(searchString)).ToListAsync());
            }
            return View(await applicationDbContext.Where(w=>w.Class== GetCurrentStudent().Grade).ToListAsync());
        }
        public async Task<IActionResult> MyAbsences(string searchString) 
        {

            var applicationDbContext = _context.Absences.
                Include(w => w.Subject).
                Include(w => w.Teacher).
                ThenInclude(w => w.ApplicationUser);

            if (!string.IsNullOrEmpty(searchString))
            {
                return View(await applicationDbContext.
                    Where(w => w.StudentID == GetCurrentStudent().ID).
                    Where(w => w.Subject.SubjectName.Contains(searchString)).ToListAsync());
            }

            return View(await applicationDbContext.Where(w=>w.StudentID==GetCurrentStudent().ID).ToListAsync());
        }


        //public async Task<IActionResult> MyRemarks() todo       BRB
        //{
        //    return View();
        //}
        
    }
}
