using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models;
using EBookStoreAPI.Models.DapperRepository;
using EBookStoreAPI.Models.EFModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBookStoreAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly BookDapperRepository _repo;
        private readonly IConfiguration _configuration;
        public BooksController(EBookStoreContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _repo = new BookDapperRepository(_context,_configuration);
        }
        //取得所有書籍
        [HttpGet]
        public async Task<IEnumerable<BooksDto>> GetBooks()
        {
            return await _repo.GetBookItems();
        }


        //取得單一書籍資訊
        [HttpGet("{id}")]
        public async Task<ActionResult<BooksDto>> GetBookById(int id)
        {
            var bookItem = await _repo.GetBookItemById(id);
            if (bookItem == null)
            {
                return NotFound(); // 如果書本不存在，回傳 404 Not Found
            }
            return bookItem;
        }

    }
}
