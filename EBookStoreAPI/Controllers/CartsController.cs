using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EBookStoreAPI.Models;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.Models.Infra.CartDapper;
using EBookStoreAPI.DTOs;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly CartGetDapperRepository _cartDapperRepository;
        private readonly CartPutDapperRepository _cartPutDapperRepository;
        private readonly CartPostDapperRepository _cartPostDapperRepository;
        private readonly CartIdGetDapperRepository _cartIdGetDapperRepository;



        public CartsController(EBookStoreContext context, CartGetDapperRepository cartDapperRepository, CartPutDapperRepository cartPutDapperRepository, CartPostDapperRepository cartPostDapperRepository, CartIdGetDapperRepository cartIdGetDapperRepository)
        {
            _context = context;
            _cartDapperRepository = cartDapperRepository;
            _cartPutDapperRepository = cartPutDapperRepository;
            _cartPostDapperRepository = cartPostDapperRepository;
            _cartIdGetDapperRepository = cartIdGetDapperRepository;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carts>>> GetCarts()
        {
            if (_context.Carts == null)
            {
                return NotFound(); 
            }

            //return await _context.Carts.ToListAsync();
            try
            {
                var carts = _cartDapperRepository.CartItemLoad();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/GetCartsList")]
        public async Task<ActionResult<Carts>> GetCartsList(CartsDto dto)
        {
            if (_context.Carts == null)
            {
                return NotFound();
            }

            //return await _context.Carts.ToListAsync();
            try
            {
                var carts = _cartIdGetDapperRepository.CartItemLoad(dto);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }

        }


        // GET: api/Carts/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<Carts>>> GetCarts(int id)
        //{
        //    if (_context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    //return await _context.Carts.ToListAsync();
        //    try
        //    {
        //        var carts = _cartIdGetDapperRepository.CartItemLoad(id);
        //        return Ok(carts);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"錯誤訊息: {ex.Message}");
        //    }
        //}


        //// PUT: api/Carts/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut/*("{id}")*/]
        //public async Task<IActionResult> PutCarts(int id, Carts carts)
        //{
        //    if (id != carts.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(carts).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CartsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut/*("{id}")*/]
        public async Task<IActionResult> PutCarts(CartsDto carts)
        {
            try
            {
                _cartPutDapperRepository.CartItemEdit(carts);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartsExists(carts.Id))
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
        //[HttpPost]
        //public async Task<ActionResult<Carts>> PostCarts(CartsDto carts)
        //{
        //    if (_context.Carts == null)
        //    {
        //        return Problem("Entity set 'EBookStoreContext.Carts'  is null.");
        //    }
        //    _context.Carts.Add(carts);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCarts", new { id = carts.Id }, carts);
        //    //return Ok("已新增至購物車");
        //}

        [HttpPost]
        public async Task<ActionResult<Carts>> PostCarts(CartsDto carts)
        {
            if (carts == null)
            {
                return Problem("Entity set 'EBookStoreContext.Carts'  is null.");
            }

            try
            {
                await _cartPostDapperRepository.CartItemPost(carts);
                return Ok("已新增至購物車");
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }

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
