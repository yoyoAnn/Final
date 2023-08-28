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
using System.Runtime.InteropServices;
using System.Configuration;
using Humanizer;
using Microsoft.AspNetCore.Cors;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class UsersController : ControllerBase
    {
        private readonly EBookStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(EBookStoreContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            //_environment = environment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
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
                //return BadRequest(new { ErrorMessage = "帳號已存在" });
            }

            //if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            //{
            //    return BadRequest("Email已存在");
            //}

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
                Password = hashOrigPwd,
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
            //var urlTemplate = $"{scheme}://8080/ActiveRegister?userid={urluserid}&confirmCode={urlconfirmCode}";
            //var urlTemplate = "https://localhost:8080/Users/ActiveRegister?userid={0}&confirmCode={1}";

            //var urlTemplate = "https://localhost:8080/ActiveRegister?userid={0}&confirmCode={1}";
            var urlTemplate = "https://127.0.0.1:8080/ActiveRegister?userid={0}&confirmCode={1}";
        
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

        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<string>> ForgetPassword(UsersDto dto)
        {

            //var user = await _context.Users.FindAsync(dto.Id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Account == dto.Account && u.Email == dto.Email);

            if (user == null)
            {
                //return NotFound();
                return BadRequest("帳號或 Email 錯誤");
            }
            if (user.IsConfirmed == false)
            {
                return BadRequest("您還未啟用帳號");
            }



            try
            {
                var urlTemplate = "https://127.0.0.1:8080/ResetPassword?userid={0}&confirmCode={1}";
                var confirmCode = Guid.NewGuid().ToString("N");
                user.ConfirmCode = confirmCode;
                await _context.SaveChangesAsync();

                var url = string.Format(urlTemplate, user.Id, confirmCode);
                new EmailHelper().SendForgetPasswordEmail(url, user.Account, user.Email);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            //await _context.SaveChangesAsync();

            return Ok("已成功記送重設密碼驗證信");
        }

        [EnableCors("AllowAll")]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<string>> ResetPassword(ChangePasswordDto dto)
        {

            //var user = await _context.Users.FindAsync(dto.Id);
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == dto.Id && u.ConfirmCode == dto.ConfirmCode);


            if (user == null)
            {
                return BadRequest("驗證錯誤，請重新寄送驗證信");
            }

            var hashPassword = HashUtility.ToSHA256(dto.NewPassword, "!@#$$DGTEGYT");

            if (dto.NewPassword != dto.ConfirmedPassword)
            {
                return BadRequest("輸入的密碼不一致，請再重新確認");
            }

            user.Password = hashPassword;
            //user.IsConfirmed = true;
            user.ConfirmCode = null;

            await _context.SaveChangesAsync();

            return Ok("重設密碼帳號驗證成功");
        }


        [HttpGet("GetUserProfileById")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return BadRequest("找不到會員資訊");
                }

                var userProfile = new
                {
                    Email = user.Email,
                    Account = user.Account,
                    Name = user.Name,
                    Phone = user.Phone,
                    Address = user.Address,
                    Gender = user.Gender
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"錯誤資訊: {ex.Message}");
            }
        }


        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile(UsersDto updatedUserDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(updatedUserDto.Id);

                if (user == null)
                {
                    return BadRequest("找不到會員資訊");
                }

                var userAccount = await _context.Users.FirstOrDefaultAsync(u => u.Id != user.Id && u.Account == updatedUserDto.Account);
                if (userAccount != null)
                {
                    return BadRequest("帳號已存在，請試試其他帳號");
                }

                user.Email = updatedUserDto.Email;
                user.Account = updatedUserDto.Account;
                user.Name = updatedUserDto.Name;
                user.Phone = updatedUserDto.Phone;
                user.Address = updatedUserDto.Address;
                user.Gender = updatedUserDto.Gender;

                await _context.SaveChangesAsync();

                return Ok("資料更新成功");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"錯誤資訊: {ex.Message}");
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword(ChangePasswordDto dto)
        {

            //var user = await _context.Users.FindAsync(dto.Id);

            var hashOrigPwd = HashUtility.ToSHA256(dto.OriginalPassword, "!@#$$DGTEGYT");
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == dto.Id && u.Password == hashOrigPwd);

            if (user == null)
            {
                return BadRequest("密碼輸入錯誤");
            }

            var hashPassword = HashUtility.ToSHA256(dto.NewPassword, "!@#$$DGTEGYT");

            if (dto.NewPassword != dto.ConfirmedPassword)
            {
                return BadRequest("輸入的密碼不一致，請再重新確認");
            }

            user.Password = hashPassword;
            await _context.SaveChangesAsync();

            return Ok("修改密碼成功");
        }


        //[HttpPost("UploadImage")]
        //public async Task<ActionResult> UploadImage()
        //{
        //    bool Results = false;
        //    try
        //    {
        //        var _uploadedfiles = Request.Form.Files;
        //        foreach (IFormFile source in _uploadedfiles)
        //        {
        //            string Filename = source.FileName;
        //            string Filepath = GetFilePath(Filename); //

        //            if (!System.IO.Directory.Exists(Filepath))
        //            {
        //                System.IO.Directory.CreateDirectory(Filepath);
        //            }

        //            string imagepath = Filepath + "\\image.png";

        //            if (System.IO.File.Exists(imagepath))
        //            {
        //                System.IO.File.Delete(imagepath);
        //            }
        //            using (FileStream stream = System.IO.File.Create(imagepath))
        //            {
        //                await source.CopyToAsync(stream);
        //                Results = true;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Ok(Results);

        //}

        //[NonAction]
        //private string GetFilePath(string ProductCode) //
        //{
        //    return this._environment.WebRootPath + "\\Uploads\\Users\\" + ProductCode; //
        //}



        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var imageFile = Request.Form.Files[0];
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, _configuration["StaticFilesConfig:ImagesFolder"]);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(imagePath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    //var user = await _context.Users.FindAsync(dto.Id);
                    //if (user == null)
                    //{
                    //    return BadRequest("找不到會員資訊");
                    //}
                    //user.Photo = uniqueFileName;
                    //await _context.SaveChangesAsync();

                   
                    var imageUrl = "https://localhost:7261/api/Users/GetImage/" + uniqueFileName;
                    return Ok(imageUrl);
                }
                else
                {
                    return BadRequest("未找到圖片");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "上傳圖片錯誤: " + ex.Message);
            }
        }


        [HttpGet("GetImage/{imageName}")]  //上傳圖片name
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, _configuration["StaticFilesConfig:ImagesFolder"], imageName);
            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/png"); 
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("UpdateUserPhoto")]  //更新資料庫name
        public async Task<IActionResult> UpdateUserPhoto(UsersDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(dto.Id);
                if (user == null)
                {
                    return BadRequest("找不到會員資訊");
                }

                user.Photo = dto.Photo;
                await _context.SaveChangesAsync();

                return Ok("圖片資料庫更新成功");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "更新圖片name錯誤: " + ex.Message);
            }
        }


        [HttpPost("GetDefaultImage")]  //DefaultImage
        public async Task<IActionResult> GetDefaultImage(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return BadRequest("找不到會員資訊");
                }

                var userProfile = new
                {
                    Photo = user.Photo,
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"錯誤資訊: {ex.Message}");
            }
        }



    }
}
