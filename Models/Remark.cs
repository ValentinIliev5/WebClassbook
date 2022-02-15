using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClassbook.Models
{
    public class Remark
    {
        public int ID { get; set; }
        public bool IsPositive { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
    }
}
