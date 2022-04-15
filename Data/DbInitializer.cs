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

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            context.Roles.Add(new IdentityRole("JEKW"));
            // Look for any students.
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }   
        }
    }
}
