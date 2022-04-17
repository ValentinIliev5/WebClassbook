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

        const int ITEMS_PER_PAGE = 5;
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
            return _context.Students.Include(w => w.ApplicationUser).First(w => w.ApplicationUserID == GetCurrentApplicationUserId());
        }


        public IActionResult Index()
        {
            //FOR MARKS WINDOW
            string avgMark = _context.Marks.Where(w => w.StudentID == GetCurrentStudent().ID).Select(w=>w.Number).DefaultIfEmpty().Average().ToString("0.00");
            
            ViewData["AvgMark"] = avgMark;


            //FOR EXAMS WINDOW
            string incExams = _context.Exams.Where(w => w.Class == GetCurrentStudent().Grade).Count(w => w.Date > DateTime.Now).ToString();
            ViewData["incExams"] = incExams;


            //FOR ABSENCES WINDOW
            string pardonedAbsences = _context.Absences.Where(w => w.StudentID == GetCurrentStudent().ID).Count(w => w.Pardoned).ToString();
            string unPardonedAbsences = _context.Absences.Where(w => w.StudentID == GetCurrentStudent().ID).Count(w => !w.Pardoned).ToString();
            ViewData["pardoned"] = pardonedAbsences;

            ViewData["unPardoned"] = unPardonedAbsences;


            //FOR REMARKS WINDOW
            string positiveRemarks = _context.Remarks.Where(w => w.StudentID == GetCurrentStudent().ID).Count(w => w.IsPositive).ToString();
            string negativeRemarks = _context.Remarks.Where(w => w.StudentID == GetCurrentStudent().ID).Count(w => !w.IsPositive).ToString();

            ViewData["positiveRemarks"] = positiveRemarks;
            ViewData["negativeRemarks"] = negativeRemarks;


            //STUDENT INFO
            ViewData["StudentInfo"] = GetCurrentStudent().ApplicationUser.Name + " - " + GetCurrentStudent().Grade;
            return View();

        }
        public async Task<IActionResult> MyMarks(string searchString, int currentPage = 1)//done
        {
            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;
            var applicationDbContext = _context.Marks.Include(w => w.Student).
               ThenInclude(w => w.ApplicationUser).
               Include(m => m.Subject).
               Include(m => m.Teacher).
               ThenInclude(w => w.ApplicationUser);

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchModel = applicationDbContext.
                    Where(w => w.StudentID == GetCurrentStudent().ID).
                    Where(w => w.Subject.SubjectName.Contains(searchString));
                ViewData["ItemsCount"] = searchModel.Count().ToString();

                return View(await searchModel.Skip((currentPage - 1)
                    * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
            }
            var studentModel = applicationDbContext.Where(w => w.StudentID == GetCurrentStudent().ID);
            ViewData["ItemsCount"] = studentModel.Count().ToString();
            return View(await studentModel.Skip((currentPage - 1)
                * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());

        }
        public async Task<IActionResult> MyExams(string searchString, int currentPage = 1) //done
        {
            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;
            var applicationDbContext = _context.Exams.Include(w => w.Subject).
                Include(w => w.Teacher).ThenInclude(w => w.ApplicationUser);

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchModel = applicationDbContext.
                    Where(w => w.Class == GetCurrentStudent().Grade).
                    Where(w => w.Subject.SubjectName.Contains(searchString));
                ViewData["ItemsCount"] = searchModel.Count().ToString();

                return View(await searchModel.Skip((currentPage - 1)
                    * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
            }

            var studentModel = applicationDbContext.Where(w => w.Class == GetCurrentStudent().Grade);
            ViewData["ItemsCount"] = studentModel.Count().ToString();
            return View(await studentModel.Skip((currentPage - 1)
                * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
        }
        public async Task<IActionResult> MyAbsences(string searchString, int currentPage = 1) //done
        {
            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;
            var applicationDbContext = _context.Absences.
                Include(w => w.Subject).
                Include(w => w.Teacher).
                ThenInclude(w => w.ApplicationUser);

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchModel = applicationDbContext.
                    Where(w => w.StudentID == GetCurrentStudent().ID).
                    Where(w => w.Subject.SubjectName.Contains(searchString));
                ViewData["ItemsCount"] = searchModel.Count().ToString();

                return View(await searchModel.Skip((currentPage - 1)
                * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
            }

            var studentModel = applicationDbContext.Where(w => w.StudentID == GetCurrentStudent().ID);
            ViewData["ItemsCount"] = studentModel.Count().ToString();

            return View(await studentModel.Skip((currentPage - 1)
                * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());
        }


        public async Task<IActionResult> MyRemarks(int currentPage=1)
        {
        
            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;

            var applicationDbContext = _context.Remarks.
                Include(w => w.Teacher).
                ThenInclude(w => w.ApplicationUser);

            var studentModel = applicationDbContext.Where(w => w.StudentID == GetCurrentStudent().ID);

            ViewData["ItemsCount"] = studentModel.Count().ToString();

            return View(await studentModel.Skip((currentPage - 1)
                * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync());

        }

    }
}
