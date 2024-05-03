using Application.Services.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomSoft.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IAuthorServices _authorServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthorController(ILogger<BookController> logger, IAuthorServices authorServices, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _authorServices = authorServices;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> GetAuthorkListAsync()
        {
            try
            {
                //var apiKey = _httpContextAccessor.HttpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
                //var apiKeySecret = _configuration["ApiKey"];

                //if (apiKey != apiKeySecret)
                //{
                //    return Unauthorized();
                //}

                var books = await _authorServices.GetAuthorListAsync();
                return Ok(JsonConvert.SerializeObject(books));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book list");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
