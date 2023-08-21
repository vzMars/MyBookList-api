using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;
using System.Security.Claims;

namespace MyBookListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<string>> GetBooks()
        {
            return Ok("get books route");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookResponse>> GetBook(string id)
        {
            var response = await _bookRepository.GetBook(id, GetUserId());

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("search/{query}")]
        public async Task<ActionResult<ICollection<Volume>>> SearchBooks(string query)
        {
            var books = await _bookRepository.SearchBooks(query);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<AddBookResponse>> AddBook(AddBookRequest request)
        {
            var response = await _bookRepository.AddBook(request, GetUserId());

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateBookStatus(int id)
        {
            return Ok("update book status route");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteBook(int id)
        {
            return Ok("delete book route");
        }
    }
}
