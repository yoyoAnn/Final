using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly EBookStoreContext _context;


        public LoginController(EBookStoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto value)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Account == value.Account && u.Password == value.Password);

            if (user == null)
            {
                return BadRequest("帳號密碼錯誤");
            }
            else
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Account),
                new Claim("FullName", user.Name),
                new Claim("UserId", user.Id.ToString()),
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                //return Ok("ok");
                return Ok(new { UserId = user.Id });
            }
        }


        [HttpDelete]
        public void logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("NoLogin")]
        public string noLogin()
        {
            return "未登入";
        }
    }
}
