using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class Mark
    {
        public int MarkID { get; set; }
        public DateTime Date { get; set; }
        public double Number { get; set; }
        public string Description { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Subject")]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        public Mark()
        {

        }
    }
}
