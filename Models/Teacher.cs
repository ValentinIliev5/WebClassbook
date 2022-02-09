using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class Teacher 
    {
        public int Id { get; set; }
        public string Grade { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<Mark> Marks { get; set; }

        public List<Absence> Absences { get; set; }

        public List<Exam> Exams { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Teacher()
        {

        }
    }
}
