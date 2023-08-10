using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models;
using EBookStoreAPI.Models.DapperRepository;
using EBookStoreAPI.Models.EFModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBookStoreAPI.Controllers
{
    [EnableCors("AllowAny")]
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
        //123
        [HttpGet]
        public async Task<IEnumerable<BooksDto>> GetBooks()
        {
            return await _repo.GetBookItems();
        }


    }
}
