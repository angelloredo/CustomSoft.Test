using Application.Commands.Book;
using Application.ViewModels.Book;
using Domain.DTO.Book;
using Microsoft.AspNetCore.Http;
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

        Task<ViewModels.Book.BookDTO> GetBookByIdAsync(Guid bookId);


        Task<List<ViewModels.Book.BookDTO>> GetBookListAsync();
        Task<ViewModels.Book.BookDTO> UploadFile(Guid bookId, IFormFile file);
        Task<(string nombre, byte[] contenido, string errorMsj)> DownloadFile(Guid bookId);


    }
}
