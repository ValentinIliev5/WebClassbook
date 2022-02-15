using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class Exam
    {
        public int ID { get; set; }

        public string Class { get; set; }

        public DateTime Date { get; set;}

        public string Description { get; set; }

        [ForeignKey("Subject")]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

    }
}
