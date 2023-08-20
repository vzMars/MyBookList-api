using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;

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

        [HttpGet]
        public async Task<ActionResult<string>> GetBooks()
        {
            return Ok("get books route");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetBook(int id)
        {
            return Ok("get book route");
        }

        [HttpGet("search/{query}")]
        public async Task<ActionResult<ICollection<Volume>>> SearchBooks(string query)
        {
            var books = await _bookRepository.SearchBooks(query);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddBook()
        {
            return Ok("add book route");
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
