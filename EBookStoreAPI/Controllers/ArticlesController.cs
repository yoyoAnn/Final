﻿using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.DapperRepository;
using EBookStoreAPI.Models.EFModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBookStoreAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly ArticleDapperRepository _articleDapperRepository;
        public ArticlesController(EBookStoreContext context, ArticleDapperRepository articleDapperRepository)
        {
            _context = context;
            _articleDapperRepository = articleDapperRepository;
        }

		// GET: api/Articles/top3
		[HttpGet("top3")]
        public async Task<ActionResult<IEnumerable<Articles>>> GetTop3Articles()
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }

            return await _context.Articles.OrderByDescending(a=>a.CreatedTime).Take(3).ToListAsync();
		}

		// GET: api/Articles
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ArticlesDto>>> GetArticles(int? writerId,string? searchText)
		{
			//if (_context.Articles == null)
			//{
			//	return NotFound();
			//}

			//return await _context.Articles.OrderByDescending(a => a.CreatedTime).ToListAsync();

            try
            {
                var articles = _articleDapperRepository.GetArticles(writerId, searchText);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

		// GET: api/Articles/5
		[HttpGet("{id}")]
        public async Task<ActionResult<ArticlesDto>> GetArticle(int id)
        {
            //if (_context.Articles == null)
            //{
            //    return NotFound();
            //}
            //var articles = await _context.Articles.FindAsync(id);

            //if (articles == null)
            //{
            //    return NotFound();
            //}

            //return articles;
            try
            {
                var article = _articleDapperRepository.GetArticle(id);
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }

        }

        // GET: api/Articles/writer/5
        [HttpGet("writer/{id}")]
        public async Task<ActionResult<WritersDto>> GetWriter(int id)
        {
            //if (_context.Articles == null)
            //{
            //    return NotFound();
            //}
            //var articles = await _context.Articles.FindAsync(id);

            //if (articles == null)
            //{
            //    return NotFound();
            //}

            //return articles;
            try
            {
                var article = _articleDapperRepository.GetWriter(id);
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }

        }
    }
}
