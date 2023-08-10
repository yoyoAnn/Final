using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.DTOs;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly EBookStoreContext _context;

        public ArticlesController(EBookStoreContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<IEnumerable<ArticlesDto>> GetArticles()
        {
          if (_context.Articles == null)
          {
              return null;
          }
            return await _context.Articles.Select(x=>new ArticlesDto (){
            
                Id = x.Id,
                BookId = x.BookId,
                BookName = x.Book.Name,
                WriterId = x.WriterId,
                WriterName = x.Writer.Name,
                Title = x.Title,
                Content = x.Content,
                PageViews = x.PageViews,
                Status = x.Status,
                CreatedTime = x.CreatedTime  
            }).ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ArticlesDto> GetArticles(int id)
        {
          if (_context.Articles == null)
          {
              return null;
          }
            //var articles = await _context.Articles.FindAsync(id);
            ArticlesDto articleDto = await _context.Articles.Where(x => x.Id == id).Select(art => new ArticlesDto
            {
                Id = art.Id,
                BookId = art.BookId,
                BookName = art.Book.Name,
                WriterId = art.WriterId,
                WriterName = art.Writer.Name,
                Title = art.Title,
                Content = art.Content,
                PageViews = art.PageViews,
                Status = art.Status,
                CreatedTime = art.CreatedTime
            }).SingleAsync();


            if (articleDto == null)
            {
                return null;
            }

            return articleDto;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutArticles(ArticlesDto articleDto)
        {
            Articles article = await _context.Articles.FindAsync(articleDto.Id);
            article.Id = articleDto.Id;
            article.WriterId = articleDto.WriterId;
            article.BookId = articleDto.BookId;
            article.Title = articleDto.Title;   
            article.Content = articleDto.Content;
            article.Status = articleDto.Status;
            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticlesExists(articleDto.Id))
                {
                    return "找不到此篇文章";
                }
                else
                {
                    throw;
                }
            }

            return "修改文章成功";
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Articles>> PostArticles(Articles articles)
        {
          if (_context.Articles == null)
          {
              return Problem("Entity set 'EBookStoreContext.Articles'  is null.");
          }
            _context.Articles.Add(articles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticles", new { id = articles.Id }, articles);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticles(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var articles = await _context.Articles.FindAsync(id);
            if (articles == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(articles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticlesExists(int id)
        {
            return (_context.Articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
