using Application.Commands.Book;
using Application.ViewModels.Book;
using Domain.DTO.Book;
using Domain.Infrastructure.Repositories.Book;
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
        public BookServices(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
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
                    BookId = updateBookCommand.id,
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
                    PublicationDate = dto.PublicationDate.Value.ToShortDateString()
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
                    PublicationDate = dto.PublicationDate.Value.ToShortDateString()
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
