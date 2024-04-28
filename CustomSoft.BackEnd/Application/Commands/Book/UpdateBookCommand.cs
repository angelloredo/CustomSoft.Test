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
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 15 caracteres")]
        public string Title { get; set; } = "";

        public string BookAuthorGuid { get; set; } = "";
        public DateTime PublicationDate { get; set; } = DateTime.Now;
    }
}
