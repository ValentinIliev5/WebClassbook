using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [ForeignKey("Student")]
        public int? StudentID { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Teacher")]
        public int? TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey("Admin")]
        public int? AdminID { get; set; }
        public Admin Admin { get; set; }
    }
}
