using Application.Commands.Book;
using Application.ViewModels.Book;
using Domain.DTO.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Book
{
    public  interface IBookServices
    {
        Task InsertBookAsync(CreateBookCommand createBookCommand);


        Task UpdateBookAsync(UpdateBookCommand updateBookCommand);



        Task DeleteBookAsync(Guid bookId);

        Task<BookViewModel> GetBookByIdAsync(Guid bookId);


        Task<List<BookViewModel>> GetBookListAsync();

    }
}
