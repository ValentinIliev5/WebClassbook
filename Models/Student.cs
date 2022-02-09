using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class Student
    {
        public int ID { get; set; }
        public int ClassNumber { get; set; }
        public string Grade { get; set; }

        [ForeignKey("Mark")]
        public List<Mark> Marks { get; set; }

        [ForeignKey("Absence")]
        public List<Absence> Absences { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Student()
        {

        }
    }
}
