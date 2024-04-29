using Application.ViewModels.Book;
using Domain.DTO.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Book
{
    public interface IAuthorServices
    {
        Task<List<BookAuthorViewModel>> GetAuthorListAsync();

    }
}
