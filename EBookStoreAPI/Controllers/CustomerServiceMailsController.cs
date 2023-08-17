using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.DapperRepository;
using Microsoft.AspNetCore.Cors;

namespace EBookStoreAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerServiceMailsController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly CSMailsDapperRepository _csMailDapperRepository;


        public CustomerServiceMailsController(EBookStoreContext context, CSMailsDapperRepository repository)
        {
            _context = context;
            _csMailDapperRepository = repository;
        }

        // GET: api/CustomerServiceMails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProblemType>>> GetCustomerServiceMails()
        {
            if (_context.CustomerServiceMails == null)
            {
                return NotFound();
            }

            return await _context.ProblemType.ToListAsync();
        }

        // GET: api/CustomerServiceMails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerServiceMails>> GetCustomerServiceMails(int id)
        {
          if (_context.CustomerServiceMails == null)
          {
              return NotFound();
          }
            var customerServiceMails = await _context.CustomerServiceMails.FindAsync(id);

            if (customerServiceMails == null)
            {
                return NotFound();
            }

            return customerServiceMails;
        }

        // PUT: api/CustomerServiceMails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerServiceMails(int id, CustomerServiceMails customerServiceMails)
        {
            if (id != customerServiceMails.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerServiceMails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerServiceMailsExists(id))
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

        // POST: api/CustomerServiceMails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		public async Task<ActionResult<string>> PostCustomerServiceMails(CSMailsDto csDto)
		{
			try
			{
				_csMailDapperRepository.AddCSMail(csDto);
				return Ok("已成功留言，謝謝您的建議，我們會盡快聯繫您。");
			}
			catch (Exception ex)
			{
				return BadRequest($"錯誤訊息: {ex.Message}");
			}

            //CustomerServiceMails entity = new CustomerServiceMails
            //{
            //    UserAccount = csDto.UserAccount,
            //    Email = csDto.Email,
            //    ProblemTypeId = csDto.ProblemTypeId,
            //    ProblemStatement = csDto.ProblemStatement,
            //    OrderId = csDto.OrderId,
            //    IsRead = false,
            //    IsReplied = false,
            //    CreatedTime = DateTime.Now
            //};

            //_context.CustomerServiceMails.Add(entity);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCustomerServiceMails", new { id = entity.Id }, entity);
        }

        // DELETE: api/CustomerServiceMails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerServiceMails(int id)
        {
            if (_context.CustomerServiceMails == null)
            {
                return NotFound();
            }
            var customerServiceMails = await _context.CustomerServiceMails.FindAsync(id);
            if (customerServiceMails == null)
            {
                return NotFound();
            }

            _context.CustomerServiceMails.Remove(customerServiceMails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerServiceMailsExists(int id)
        {
            return (_context.CustomerServiceMails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
