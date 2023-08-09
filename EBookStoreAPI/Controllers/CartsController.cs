using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EBookStoreAPI.Models;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly EBookStoreContext _context;

        public CartsController(EBookStoreContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carts>>> GetCarts()
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            return await _context.Carts.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carts>> GetCarts(int id)
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            var carts = await _context.Carts.FindAsync(id);

            if (carts == null)
            {
                return NotFound();
            }

            return carts;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarts(int id, Carts carts)
        {
            if (id != carts.Id)
            {
                return BadRequest();
            }

            _context.Entry(carts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Carts>> PostCarts(Carts carts)
        {
          if (_context.Carts == null)
          {
              return Problem("Entity set 'EBookStoreContext.Carts'  is null.");
          }
            _context.Carts.Add(carts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarts", new { id = carts.Id }, carts);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarts(int id)
        {
            if (_context.Carts == null)
            {
                return NotFound();
            }
            var carts = await _context.Carts.FindAsync(id);
            if (carts == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(carts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartsExists(int id)
        {
            return (_context.Carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
