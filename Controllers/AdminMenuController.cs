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
    [Authorize(Roles = "Admin")]
    public class AdminMenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminMenuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> Users = new List<ApplicationUser>();

            foreach (ApplicationUser user in _context.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count == 1 && roles.First() == "User")
                {
                    Users.Add(user);
                }
            }


            ViewData["needApproval"] = Users.Count().ToString();
            return View();
        }
    }
}
