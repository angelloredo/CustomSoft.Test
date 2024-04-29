using Application.ViewModels.Book;
using Domain.Infrastructure.Repositories.Book;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Book
{
    public class AuthorServices : IAuthorServices
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IHostEnvironment _hostEnvironment;
        public AuthorServices(IAuthorRepository authorRepository, IHostEnvironment hostEnvironment)
        {
            _authorRepository = authorRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<List<BookAuthorDTO>> GetAuthorListAsync()
        {
            try
            {
                var list = await _authorRepository.GetBookAuthorList();

                return list.Select(dto => new BookAuthorDTO
                {
                    BookAuthorGuid = dto.BookAuthorGuid,
                    Name = $"{dto.Name} {dto.LastName}",
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
