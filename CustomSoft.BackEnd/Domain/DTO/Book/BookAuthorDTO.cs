using Domain.Entities.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Book
{
    public class BookAuthorDTO
    {
        public required int BookAuthorId { get; set; }
        public required string BookAuthorGuid { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public required IEnumerable<AcademicGradeDTO> AcademicGrades { get; set; }
    }
}
