using System;
using System.Collections.Generic;
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

        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        public Mark()
        {

        }
    }
}
