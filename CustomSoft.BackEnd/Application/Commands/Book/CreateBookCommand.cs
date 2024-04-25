using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Book
{
    public class CreateBookCommand
    {
        public string title { get; set; } = "";
        public string bookAuthorGuid { get; set; } = "";
        public DateTime publicationDate { get; set; } = DateTime.Now;
    }
}
