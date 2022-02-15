using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }

        public string SubjectName { get; set; }

        public List<Teacher> Teachers { get; set; }

       public List<Mark> Marks { get; set; }
       public List<Absence> Absences { get; set; }
       public List<Exam> Exams { get; set; }
       
    }
}
