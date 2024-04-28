using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Book
{
    public class BookDTO
    {
        public required Guid BookId { get; set; }
        public required string Title { get; set; }
        public string? FileExtension { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string BookAuthorGuid { get; set; }
        public string AuthorName { get; set; }
        public byte[] File{ get; set; }
        public string AuthorLastName { get; set; }
        public DateTime? AuthorBirthdate { get; set; }
        public string? FileName { get; set; }

    }
}
