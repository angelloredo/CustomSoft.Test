using Application.Commands.Book;
using Application.ViewModels.Book;
using Domain.DTO.Book;
using Domain.Infrastructure.Repositories.Book;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDTO = Application.ViewModels.Book.BookDTO;

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

        public async Task<ViewModels.Book.BookDTO> GetBookByIdAsync(Guid bookId)
        {
            try
            {
                Domain.DTO.Book.BookDTO dto = await _bookRepository.GetBookByIdAsync(bookId);

                return new ViewModels.Book.BookDTO
                {
                    BookId = bookId,
                    Title = dto.Title,
                    BookAuthorGuid = dto.BookAuthorGuid,
                    FileName = dto.FileName,
                    AuthorName = dto.AuthorName,
                    AuthorLastName = dto.AuthorLastName,
                    PublicationDate = dto.PublicationDate != null ? dto.PublicationDate.Value.ToString("yyyy-MM-dd") : "Sin publicar."
                };
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<ViewModels.Book.BookDTO>> GetBookListAsync()
        {
            try
            {
                var list = await _bookRepository.GetBookListAsync();

                return list.Select(dto => new BookDTO
                {
                    BookId = dto.BookId,
                    Title = dto.Title,
                    BookAuthorGuid = dto.BookAuthorGuid,
                    AuthorName = dto.AuthorName,
                    AuthorLastName = dto.AuthorLastName,
                    FileName = dto.FileName,
                    PublicationDate = dto.PublicationDate != null ? dto.PublicationDate.Value.ToString("yyyy-MM-dd") : "Sin publicar."
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ViewModels.Book.BookDTO> UploadFile(Guid bookId, IFormFile file)
        {
            try
            {

                // Obtener información sobre el archivo
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{bookId}{fileExtension}";
                var fileSize = file.Length;
                byte[] fileBytes;


                var book = await _bookRepository.GetBookByIdAsync(bookId);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                await _bookRepository.UpdateBookAsync(new Domain.Entities.Book.Book
                {
                    BookId = bookId,
                    Title = book.Title,
                    FileName = file.FileName,
                    BookAuthorGuid = book.BookAuthorGuid,
                    PublicationDate = book.PublicationDate,
                    FileSize = fileSize.ToString(),
                    FileExtension = fileExtension,
                    File = fileBytes
                });

                return new ViewModels.Book.BookDTO
                {
                    BookId = bookId,
                    FileName = fileName
                };
            }
            catch (Exception ex)
            {
                return new ViewModels.Book.BookDTO
                {
                    ErrorMsj = ex.Message
                };
            }
        }

        public async Task<(string nombre, byte[] contenido, string errorMsj)> DownloadFile(Guid bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);

                if (book.File == null)
                    return ("", null, "No existe el archivo.");

                var nombre = book.FileName;
                var contenido = book.File;

             
                return (nombre, contenido, "");
            }
            catch (Exception ex)
            {
                return ("", null, ex.Message);
            }
        }
    }
}