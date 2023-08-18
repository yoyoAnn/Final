using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EBookStoreAPI.Models;
using EBookStoreAPI.Models.EFModels;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using EBookStoreAPI.DTOs.Users;
using NuGet.Common;
using EBookStoreAPI.Models.Infra;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;


namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(EBookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Users
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'EBookStoreContext.Users'  is null.");
          }
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("GetUserId")]
        public IActionResult GetUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim != null)
                {
                    int userId = Convert.ToInt32(userIdClaim.Value);
                    return Ok(userId);
                }
                else
                {
                    return BadRequest("UserId claim not found.");
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }


        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
     
            if (await _context.Users.AnyAsync(u => u.Account == registerDto.Account))
            {
                return BadRequest("帳號已存在"); 
            }

            if (registerDto.Password != registerDto.ConfirmedPassword)
            {
                return BadRequest("輸入的密碼不一致，請再重新確認");
            }

            var hashOrigPwd = HashUtility.ToSHA256(registerDto.Password, "!@#$$DGTEGYT");

            registerDto.ConfirmedPassword = null;
            var confirmCode = Guid.NewGuid().ToString("N");

            Users user = new Users
            {
                Id = registerDto.Id,
                Account = registerDto.Account,
                Password = registerDto.Password, 
                Email = registerDto.Email,
                IsConfirmed = false,
                ConfirmCode = confirmCode,
                CreatedTime = DateTime.Now,
            };

            //_context.Users.Add(user);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();


            //var urlTemplate = Request.Url.Scheme + "://" +  // 生成 http:.// 或 https://
            //                 Request.Url.Authority + "/" + // 生成網域名稱或 ip
            //                 "Users/ResetPassword?userId={0}&confirmCode={1}";

            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host.ToUriComponent();
            //var urluserid = user.Id;
            //var urlconfirmCode = confirmCode;
            //var urlTemplate = $"{scheme}://5173/ActiveRegister?userid={urluserid}&confirmCode={urlconfirmCode}";
            //var urlTemplate = "https://localhost:5173/Users/ActiveRegister?userid={0}&confirmCode={1}";
            var urlTemplate = "https://localhost:5173/ActiveRegister?userid={0}&confirmCode={1}";

            try
            {
                // 寄送email
                //var url = string.Format(urlTemplate, user.Id, confirmCode);
                var url = string.Format(urlTemplate, user.Id, confirmCode);
                new EmailHelper().SendConfirmRegisterEmail(url, user.Account, user.Email);

                //return CreatedAtAction("GetUser", new { id = user.Id }, user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(ex.Message);

            }
           
            return Ok("註冊成功");
        }

        [HttpPost("ActiveRegister")]
        public async Task<ActionResult<string>> ActiveRegister(UsersDto dto)
        {
                       
            var user = await _context.Users.FindAsync(dto.Id);

            user.IsConfirmed = true;
            user.ConfirmCode = null;

            await _context.SaveChangesAsync();

            return Ok("帳號啟用成功");

        }
    }
}
