using Dapper;
using Domain.DTO.Book;
using Domain.Entities.Book;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Infrastructure.Repositories.Book
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task InsertBookAsync(Domain.Entities.Book.Book book)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        p_BookId = Guid.NewGuid(),
                        p_Title = book.Title,
                        p_FileExtension = book.FileExtension,
                        p_FileName = book.FileName,
                        p_FileDirection = book.FileDirection,
                        p_PublicationDate = book.PublicationDate,
                        p_BookAuthorGuid = book.BookAuthorGuid
                    };

                    await connection.ExecuteAsync("SELECT InsertBook(@p_BookId, @p_Title, @p_FileExtension, @p_FileName, @p_FileDirection, @p_PublicationDate, @p_BookAuthorGuid)", parameters);


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateBookAsync(Domain.Entities.Book.Book book)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        p_Title = book.Title,
                        p_FileExtension = book.FileExtension,
                        p_FileName = book.FileName,
                        p_FileDirection = book.FileDirection,
                        p_PublicationDate = book.PublicationDate,
                        p_BookAuthorGuid = book.BookAuthorGuid,
                        p_BookId = book.BookId // Asegúrate de incluir el ID del libro
                    };

                    await connection.ExecuteAsync("SELECT UpdateBook(@p_BookId, @p_Title, @p_FileExtension, @p_FileName, @p_FileDirection, @p_PublicationDate, @p_BookAuthorGuid)", parameters);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí
                throw;
            }
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        p_BookId = bookId
                    };

                    await connection.ExecuteAsync("SELECT DeleteBook(@p_BookId)", parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<BookDTO> GetBookByIdAsync(Guid bookId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new { p_BookId = bookId };

                    var result = await connection.QuerySingleAsync<BookDTO>("SELECT * FROM get_book_info(@p_BookId)", parameters);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IEnumerable<BookDTO>> GetBookListAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<BookDTO>("SELECT * FROM get_book_list()");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
