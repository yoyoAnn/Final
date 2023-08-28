using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.Models.Infra;
using Google.Apis.Auth;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Text;



namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowAll")]
    public class LoginController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly IConfiguration _configuration;


        public LoginController(EBookStoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto value)
        {
            var hashOrigPwd = HashUtility.ToSHA256(value.Password, "!@#$$DGTEGYT");

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Account == value.Account && u.Password == hashOrigPwd);

            if (user == null)
            {
                return BadRequest("帳號或密碼錯誤");
            }
            else if (user.IsConfirmed != true) {
                return BadRequest("帳號尚未啟用，請先至註冊的Email啟用帳號");
            }
            else 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Account),
                    //new Claim("FullName", user.Name),
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


        [EnableCors("AllowAll")]
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] string credential)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration["GoogleApiClientId"] }
                };

                var googleuser = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

                if (googleuser != null)
                {
                    var hadUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == googleuser.Email);

                    if (hadUser == null)
                    {
                        var newGoogleUser = new Users
                        {
                            Name = googleuser.Name,
                            Account = googleuser.Email,
                            Password = "F5478EBD0ECCDE7FB5911BB0C1D4BE0E0422FE150E3E4165338CE3A74B6BF439",
                            Email = googleuser.Email,
                            Photo = "defaultUser.jpg",
                            IsConfirmed = true,
                            CreatedTime = DateTime.Now,
                        };
                        await _context.Users.AddAsync(newGoogleUser);
                        await _context.SaveChangesAsync();
                    }
                    
                    //var hashedPassword = HashUtility.ToSHA256(hadUser.Password, "!@#$$DGTEGYT");
                    var loginDto = new LoginDto
                    {
                        Account = googleuser.Email,
                        Password = "123"
                    };
                    
                    return await Login(loginDto);
                }
                else
                {
                    return BadRequest(new { message = "Google登入失敗" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Google登入失敗", error = ex.Message });
            }
        }

    }
}
