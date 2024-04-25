using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Book
{
    public class BookAuthorViewModel
    {
        public required int BookAuthorId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Birthdate { get; set; }
        public required List<AcademicGradeViewModel> AcademicGrades { get; set; }


    }
}
