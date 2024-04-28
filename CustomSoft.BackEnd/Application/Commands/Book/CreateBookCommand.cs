using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Book
{
    public class CreateBookCommand
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 15 caracteres")]
        public string title { get; set; } = "";

        [Required(ErrorMessage = "El ID del autor del libro es requerido")]
        public string bookAuthorGuid { get; set; } = "";

        [Required(ErrorMessage = "La fecha de publicación es requerida")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de publicación debe ser de tipo fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime publicationDate { get; set; } = DateTime.Now;
    }
}
