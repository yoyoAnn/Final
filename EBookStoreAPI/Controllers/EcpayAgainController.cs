using EBookStoreAPI.DTOs.Orders;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.Models.Infra.CartDapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using XSystem.Security.Cryptography;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcpayAgainController : ControllerBase
    {

        private readonly OrderStatusEditDapperRepository _orderStatusEditDapperRepository;
        private readonly OrderIdEditDapperRepository _orderIdEditDapperRepository;

        public EcpayAgainController(OrderStatusEditDapperRepository orderStatusEditDapperRepository, OrderIdEditDapperRepository orderIdEditDapperRepository)
        {
            _orderStatusEditDapperRepository = orderStatusEditDapperRepository;
            _orderIdEditDapperRepository = orderIdEditDapperRepository;
        }
        string orderId = "";

        [HttpPost("EcpayAgain")]
        public ActionResult<IDictionary<string, string>> GetOrderDetails(IEnumerable<EcPayAgain> dto)
        {
            orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = $"https://127.0.0.1:8081/";

            int totalAmountTemp = 80;
            string totalItemName = string.Empty;

            string oldorderid = dto.FirstOrDefault().orderId;


            foreach (var item in dto)
            {
                totalAmountTemp += item.price * item.qty;
                totalItemName += item.name + " " + "x" + " " + item.qty + "#";
            }

            if (totalItemName.Length > 0)
            {
                totalItemName = totalItemName.Substring(0, totalItemName.Length - 1);
            }

            string totalAmount = totalAmountTemp.ToString();

            var order = new Dictionary<string, string>
            {
                    //綠界需要的參數
                    { "MerchantTradeNo",  orderId},//特店訂單編號
                    { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},//特店交易時間
                    { "TotalAmount",  totalAmount},//交易金額
                    { "TradeDesc",  "無"},//交易描述
                    { "ItemName",  totalItemName},//商品名稱 如果商品名稱有多筆，需在金流選擇頁一行一行顯示商品名稱的話，商品名稱請以符號#分隔。

                    { "ReturnURL",  $"https://localhost:7261/api/EcpayAgain/EcpayReturnAgain/{orderId}"},//回傳網址
                    { "OrderResultURL",$"https://localhost:7261/api/EcpayAgain/EcpayReturnAgain/{orderId}"},//交易結果回傳頁面
                    //{ "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
                    //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
                    { "MerchantID",  "3002607"},//特店編號
                    { "IgnorePayment",  "TWQR#WebATM#ATM#CVS#BARCODE"},
                    { "PaymentType",  "aio"},//交易類型
                    { "ChoosePayment",  "ALL"},//付款方式
                    { "EncryptType",  "1"},//加密類型
                    {"ClientBackURL", $"{website}orders" }//付款完成通知回傳網址
            };

            order["CheckMacValue"] = GetCheckMacValue(order);//檢查碼

            var editdto = new oldNewOrderId();
            editdto.oldOrderid = oldorderid;
            editdto.newOrderid = orderId;

            _orderIdEditDapperRepository.OrderIdEditPost(editdto);

            return Ok(order);
        }

        [HttpPost("EcpayReturnAgain/{orderId}")]
        public IActionResult EcpayReturn([FromForm] EcpayReturnDto info)
        {
            if (info.RtnMsg == "Succeeded")
            {
                _orderStatusEditDapperRepository.PayInfoEdit(info);



                return Redirect($"https://127.0.0.1:8081/orders/");
            }
            else
            {
                return Ok(info.RtnMsg);
            }

        }


        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);

            var hashKey = "pwFHCqoQZGmho4w6";
            var HashIV = "EkRm7iFT261dpevs";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = WebUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            using var sha256 = new SHA256Managed();  // 使用直接建構的方式來創建 SHA256Managed 實例
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        //[HttpPost]
        //[Route("/CartAddToOrderDBAgain")]
        //public async Task<ActionResult> CartAddToOrderDB(OrdersDto dto)
        //{
        //    if (_context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    return await _context.Carts.ToListAsync();
        //    try
        //    {
        //        var carts = _orderPostDapperRepository.PayInfoPost(dto);
        //        return Ok(carts);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"錯誤訊息: {ex.Message}");
        //    }
        //}

        //[HttpPost]
        //[Route("/CartAddToOrderItemsDBAgain")]
        //public async Task<ActionResult> CartAddToOrderItemsDB(OrderItemsDto dto)
        //{
        //    if (_context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    return await _context.Carts.ToListAsync();
        //    try
        //    {
        //        var carts = _orderItemPostDapperRepository.OrderItemPost(dto);
        //        return Ok(carts);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"錯誤訊息: {ex.Message}");
        //    }
        //}


        //[HttpPost]
        //[Route("/PaymentCartAgain/{id}")]
        //public async Task<ActionResult> PaymentCart(int id)
        //{
        //    try
        //    {
        //        await _paymentCartDapperRepository.PaymentCartEdit(id);
        //        return Ok($"編號{id}已完成更新");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"錯誤訊息: {ex.Message}");
        //    }

        //}

    }
}
