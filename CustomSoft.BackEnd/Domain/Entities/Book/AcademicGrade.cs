using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Book
{
    public class AcademicGrade
    {
        public int AcademicGradeId { get; set; }
        public required string AcademicGradeGuid { get; set; }
        public string? Name { get; set; }
        public string? AcademicCenter { get; set; }
        public DateTime? FechaGrado { get; set; }
        public int BookAuthorId { get; set; }
        public BookAuthor? BookAuthor { get; set; }

    }
}
