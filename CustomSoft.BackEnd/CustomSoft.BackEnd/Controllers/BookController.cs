using Application.Commands.Book;
using Application.Services.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;
using Serilog;
using System;

namespace CustomSoft.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookServices _bookService; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookController(ILogger<BookController> logger, IBookServices bookService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> InsertBookAsync(CreateBookCommand createBookCommand)
        {
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
                var apiKey = _httpContextAccessor.HttpContext.Request.Headers["X-Api-Key"].FirstOrDefault();

                if (apiKey != "my-secret-api-key")
                {
                    return Unauthorized();
                }

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

                // Obtener información sobre el archivo
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{bookId}{fileExtension}";
                var fileSize = file.Length;

                // Guardar el archivo en el servidor (aquí puedes almacenarlo en la ubicación que desees)
                // Por ejemplo, puedes guardarlos en la carpeta "uploads" en la raíz del proyecto
                var filePath = Path.Combine("uploads", fileName);
                //var filePath = $".../../files/{fileName}" + fileExtension;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Devolver información sobre el archivo cargado
                var fileInfo = new
                {
                    ClientId = bookId,
                    FileName = fileName,
                    FileSize = fileSize,
                    FileExtension = fileExtension,
                    FilePath = filePath // Puedes devolver la ruta del archivo si lo deseas
                };

                return Ok(fileInfo);
            }
            catch (Exception ex)
            {
                return Ok($"Ocurrió un error al cargar el archivo: {ex.Message}");
            }
        }

        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            // Ruta donde se encuentra el archivo (ajústala según tu estructura de archivos)
            var filePath = Path.Combine("uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo solicitado no existe.");
            }

            // Obtener el tipo MIME del archivo
            var contentType = MimeTypes.GetMimeType(filePath);

            // Leer el archivo en bytes
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Retornar el archivo como un FileStreamResult
            return File(fileBytes, contentType, fileName);
        }
    }


}
