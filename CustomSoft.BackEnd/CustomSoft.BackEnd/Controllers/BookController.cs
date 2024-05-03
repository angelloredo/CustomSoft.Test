using Application.Commands.Book;
using Application.Services.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;
using Serilog;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomSoft.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookServices _bookService; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public BookController(ILogger<BookController> logger, IBookServices bookService, IHttpContextAccessor httpContextAccessor , IConfiguration configuration)
        {
            _logger = logger;
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> InsertBookAsync(CreateBookCommand createBookCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bookService.InsertBookAsync(createBookCommand);
                _logger.LogInformation("Book inserted successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting book");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookAsync(UpdateBookCommand updateBookCommand)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                await _bookService.UpdateBookAsync(updateBookCommand);
                _logger.LogInformation("Book updated successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookAsync(Guid bookId)
        {
            try
            {
                await _bookService.DeleteBookAsync(bookId);
                _logger.LogInformation("Book deleted successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookByIdAsync(Guid bookId)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(bookId);
                return Ok(JsonConvert.SerializeObject(book));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book by ID");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBookListAsync()
        {
            try
            {
                //var apiKey = _httpContextAccessor.HttpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
                //var apiKeySecret = _configuration["ApiKey"];

                //if (apiKey != apiKeySecret)
                //{
                //    _logger.LogError($"No autorizado.");
                //    return Unauthorized();
                //}

                var books = await _bookService.GetBookListAsync();
                return Ok(JsonConvert.SerializeObject(books));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book list");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }



        [HttpPost]
        [Route("upload/{bookId}")]
        public async Task<IActionResult> Upload(Guid bookId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No se ha proporcionado ningún archivo o el archivo está vacío.");

                var result = await _bookService.UploadFile(bookId, file);

                if (result.ErrorMsj != null && result.ErrorMsj.Length > 0)
                    _logger.LogError($"Ocurrió un error al cargar el archivo: {result.ErrorMsj}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al cargar el archivo: {ex.Message}");
                return Problem($"Ocurrió un error al cargar el archivo: {ex.Message}");
            }
        }

        [HttpGet("download/{bookId}")]
        public async Task<IActionResult> DownloadFile(Guid bookId)
        {
            try
            {
                var res = await _bookService.DownloadFile(bookId);

                if (res.contenido == null)
                    return BadRequest($"Ocurrió un error al descargar el archivo: {res.errorMsj}");

                return File(res.contenido, "application/octet-stream", res.nombre);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al descargar el archivo: {ex.Message}");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
          
        }
    }


}
