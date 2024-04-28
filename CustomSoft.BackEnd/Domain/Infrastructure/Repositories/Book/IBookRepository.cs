using Domain.DTO.Book;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Repositories.Book
{
    public interface IBookRepository
    {
        Task InsertBookAsync(Domain.Entities.Book.Book book);
        Task UpdateBookAsync(Domain.Entities.Book.Book book);
        Task DeleteBookAsync(Guid bookId);
        Task<BookDTO> GetBookByIdAsync(Guid bookId);
        Task<IEnumerable<BookDTO>> GetBookListAsync();

    }
}
