using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using XSystem.Security.Cryptography;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcpayController : ControllerBase
    {
        [HttpGet("GetOrderDetails")]
        public ActionResult<IDictionary<string, string>> GetOrderDetails()
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = $"https://localhost:44325/";
            var order = new Dictionary<string, string>
            {
                // ... your dictionary initialization here ...
            };

            order["CheckMacValue"] = GetCheckMacValue(order);
            return Ok(order);
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);

            var hashKey = "5294y06JbISpM5x9";
            var HashIV = "v77hoKGq4kWxNNIS";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = WebUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            using var sha256 = SHA256.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
