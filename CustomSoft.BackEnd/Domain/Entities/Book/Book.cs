using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Book
{
    public class Book
    {
        public required Guid BookId { get; set; }
        public required string Title { get; set; }
        public  string? FileExtension { get; set; }
        public  string? FileName { get; set; }
        public  string? FileSize { get; set; }
        public byte[]? File {  get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? BookAuthorGuid { get; set; }
    }
}
