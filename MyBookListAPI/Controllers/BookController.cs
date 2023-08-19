using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyBookListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
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
        public async Task<ActionResult<string>> SearchBooks(string query)
        {
            return Ok("search books route");
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
