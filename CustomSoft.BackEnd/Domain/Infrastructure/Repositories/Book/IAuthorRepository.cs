using Domain.DTO.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Repositories.Book
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<BookAuthorDTO>> GetBookAuthorList();
        Task UpdateBookAuthorAsync(int bookAuthorId, string bookAuthorGuid, string name, string lastName, DateTime birthdate);
    }
}
