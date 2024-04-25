using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Book
{
    public class BookViewModel
    {
        public BookViewModel()
        {

        }
        public  Guid BookId { get; set; }
        public  string Title { get; set; }
        public string? FileDirection { get; set; }
        public string? FileName { get; set; }
        public string? PublicationDate { get; set; }
        public string BookAuthorGuid { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLastName { get; set; }
        public string? Birthdate { get; set; } 
        public string? ErrorMsj { get; set; } 
    }
}
