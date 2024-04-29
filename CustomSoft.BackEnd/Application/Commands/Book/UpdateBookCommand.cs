using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Book
{
    public class UpdateBookCommand
    {
        [Required]
        public Guid bookId { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 100 caracteres")]
        public string title { get; set; } = "";

        [Required(ErrorMessage = "El ID del autor del libro es requerido")]
        public string bookAuthorGuid { get; set; } = "";

        public DateTime? publicationDate { get; set; }
    }
}
