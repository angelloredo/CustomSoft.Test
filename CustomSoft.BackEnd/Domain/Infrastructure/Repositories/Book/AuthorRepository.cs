using Dapper;
using Domain.DTO.Book;
using Domain.Entities.Book;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Repositories.Book
{
    public class AuthorRepository : IAuthorRepository
    {

        private readonly string _connectionString;

        public AuthorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertBookAuthor(string bookAuthorGuid, string name, string lastname, DateTime birthdate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        BookAuthorGuid = bookAuthorGuid,
                        Name = name,
                        LastName = lastname,
                        Birthdate = birthdate
                    };

                    await connection.ExecuteAsync("SELECT insert_book_author(@BookAuthorGuid, @Name, @LastName, @Birthdate)", parameters);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        public async Task UpdateBookAuthorAsync(int bookAuthorId, string bookAuthorGuid, string name, string lastName, DateTime birthdate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        p_book_author_id = bookAuthorId,
                        p_book_author_guid = bookAuthorGuid,
                        p_name = name,
                        p_lastname = lastName,
                        p_birthdate = birthdate
                    };

                    await connection.ExecuteAsync("SELECT update_book_author(@p_book_author_id, @p_book_author_guid, @p_name, @p_lastname, @p_birthdate)", parameters);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteBookAuthor(int bookAuthorId)
        {

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        BookAuthorId = bookAuthorId
                    };

                    await connection.ExecuteAsync("SELECT delete_book_author(@BookAuthorId)", parameters);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BookAuthorDTO>> GetBookAuthorList()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<BookAuthorDTO>("SELECT * FROM get_book_author_list()");
            }
        }

        public async Task<BookAuthorDTO?> GetAuthorWithAcademicGrades(int bookAuthorId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Get the author by ID
                    var author = await connection.QuerySingleAsync<BookAuthorDTO>("SELECT * FROM GetAuthorById(@p_BookAuthorId)", new { p_BookAuthorId = bookAuthorId });

                    if (author != null)
                    {
                        // Get the academic grades by autho
                        author.AcademicGrades = await connection.QueryAsync<AcademicGradeDTO>("SELECT * FROM GetAcademicGradesByAuthor(@p_BookAuthorId)", new { p_BookAuthorId = bookAuthorId });
                    }

                    return author;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
