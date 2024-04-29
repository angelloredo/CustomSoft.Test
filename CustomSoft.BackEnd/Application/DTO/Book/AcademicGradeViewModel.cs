using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Book
{
    public class AcademicGradeViewModel
    {
        public int AcademicGradeId { get; set; }
        public string? Name { get; set; }
        public string? AcademicCenter { get; set; }
        public string? FechaGrado { get; set; }
        public int BookAuthorId { get; set; }
    }
}
