using Application.Commands.Book;
using Application.ViewModels.Book;
using Domain.DTO.Book;
using Domain.Infrastructure.Repositories.Book;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Book
{
    public class BookServices : IBookServices
    {

        private readonly IBookRepository _bookRepository;
        private readonly IHostEnvironment _hostEnvironment;
        public BookServices(IBookRepository bookRepository, IHostEnvironment hostEnvironment)
        {
            _bookRepository = bookRepository;
            _hostEnvironment = hostEnvironment;
        }
        public async Task InsertBookAsync(CreateBookCommand createBookCommand)
        {
            try
            {

                await _bookRepository.InsertBookAsync(new Domain.Entities.Book.Book
                {
                    BookId = new Guid(),
                    Title = createBookCommand.title,
                    BookAuthorGuid = createBookCommand.bookAuthorGuid,
                    PublicationDate = createBookCommand.publicationDate
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task UpdateBookAsync(UpdateBookCommand updateBookCommand)
        {
            try
            {
                await _bookRepository.UpdateBookAsync(new Domain.Entities.Book.Book
                {
                    BookId = updateBookCommand.bookId,
                    Title = updateBookCommand.title,
                    BookAuthorGuid = updateBookCommand.bookAuthorGuid,
                    PublicationDate = updateBookCommand.publicationDate
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task DeleteBookAsync(Guid bookId)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(bookId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<BookViewModel> GetBookByIdAsync(Guid bookId)
        {
            try
            {
                BookDTO dto = await _bookRepository.GetBookByIdAsync(bookId);

                return new BookViewModel
                {
                    BookId = bookId,
                    Title = dto.Title,
                    BookAuthorGuid = dto.BookAuthorGuid,
                    AuthorName = dto.AuthorName,
                    AuthorLastName = dto.AuthorLastName,
                    PublicationDate = dto.PublicationDate.Value.ToString("yyyy-MM-dd")
                };
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<BookViewModel>> GetBookListAsync()
        {
            try
            {
                var list = await _bookRepository.GetBookListAsync();

                return list.Select(dto => new BookViewModel
                {
                    BookId = dto.BookId,
                    Title = dto.Title,
                    BookAuthorGuid = dto.BookAuthorGuid,
                    AuthorName = dto.AuthorName,
                    AuthorLastName = dto.AuthorLastName,
                    PublicationDate = dto.PublicationDate.Value.ToString("yyyy-MM-dd")
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<BookViewModel> UploadFile(Guid bookId, IFormFile file)
        {
            try
            {

                // Obtener información sobre el archivo
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{bookId}{fileExtension}";
                var fileSize = file.Length;

                //otener directorio en el que se guardara el archivo
                var filePath = Path.Combine("Uploads", fileName);


                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var book = await _bookRepository.GetBookByIdAsync(bookId);

                await _bookRepository.UpdateBookAsync(new Domain.Entities.Book.Book
                {
                    BookId = bookId,
                    Title = book.Title,
                    BookAuthorGuid = book.BookAuthorGuid,
                    PublicationDate = book.PublicationDate,
                    FileDirection = filePath,
                    FileSize = fileSize.ToString(),
                    FileExtension = fileExtension,
                });

                return new BookViewModel
                {
                    BookId = bookId,
                    FileName = fileName
                };
            }
            catch (Exception ex)
            {
                return new BookViewModel
                {
                    ErrorMsj = ex.Message
                };
            }
        }
    }
}