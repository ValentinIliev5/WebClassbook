using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClassbook.Models;

namespace WebClassbook.Data
{
    public static class DbInitializer
    {

        public static async void InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Admins.Any())
            {
                return;   // DB has been seeded
            }


            Admin admin = new Admin();
            var user = new ApplicationUser { UserName = "admin", Email = "admin@abv.bg", PhoneNumber = "0887078057", Name = "Admin Adminov" };
            var result = await _userManager.CreateAsync(user, "A-dmin1");
            if (result.Succeeded)
            { 
                admin.ApplicationUserID=user.Id;
                context.Admins.Add(admin);
                context.SaveChanges();
                context.Users.First(w => w.Id == user.Id).AdminID = context.Admins.First(w => w.ApplicationUserID == user.Id).ID;
                
                context.SaveChanges();
            }
                
        }
    }
}
