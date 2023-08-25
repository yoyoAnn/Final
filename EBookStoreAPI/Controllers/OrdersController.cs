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
using EBookStoreAPI.DTOs.Orders;
using EBookStoreAPI.VM.OrderVM;
using EBookStoreAPI.Models.Infra;

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderNotPayDapperRepository _orderNotPayDapperRepository;
        private readonly OrderItemsDapperRepository _orderItemsDapperRepository;
        private readonly OrderPayDapperRepository _orderPayDapperRepository;
        private readonly CancelOrderIdDapperRepository _cancelOrderIdDapperRepository;
        private readonly OrderCancelDapperRepository _orderCancelDapperRepository;
        private readonly OrderFinalDapperRepository _orderFinalDapperRepository;
        private readonly OrderReturnDapperRepository _orderReturnDapperRepository;





        public OrdersController(OrderNotPayDapperRepository orderNotPayDapperRepository, OrderItemsDapperRepository orderItemsDapperRepository, OrderPayDapperRepository orderPayDapperRepository, CancelOrderIdDapperRepository cancelOrderIdDapperRepository, OrderCancelDapperRepository orderCancelDapperRepository, OrderFinalDapperRepository orderFinalDapperRepository, OrderReturnDapperRepository orderReturnDapperRepository)
        {
            _orderNotPayDapperRepository = orderNotPayDapperRepository;
            _orderItemsDapperRepository = orderItemsDapperRepository;
            _orderPayDapperRepository = orderPayDapperRepository;
            _cancelOrderIdDapperRepository = cancelOrderIdDapperRepository;
            _orderCancelDapperRepository = orderCancelDapperRepository;
            _orderFinalDapperRepository = orderFinalDapperRepository;
            _orderReturnDapperRepository = orderReturnDapperRepository;
        }

        [HttpPost]
        [Route("/GetOrderNotPayList")]
        public async Task<ActionResult<Carts>> GetCartsList(GetOrdersListId dto)
        {
            //return await _context.Carts.ToListAsync();
            try
            {
                var orders = _orderNotPayDapperRepository.OrderItemLoad(dto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/GetOrderPaidList")]
        public async Task<ActionResult<Carts>> GetPayCartsList(GetOrdersListId dto)
        {
            //return await _context.Carts.ToListAsync();
            try
            {
                var orders = _orderPayDapperRepository.OrderPaidLoad(dto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/GetOrderItemList")]
        public async Task<ActionResult<OrderItemsVM>> GetOrderItemList(GetOrderItem dto)
        {

            //return await _context.Carts.ToListAsync();
            try
            {
                var orders = _orderItemsDapperRepository.GetOrderItemLoad(dto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }

        }

        [HttpPost]
        [Route("/CancelOrder")]
        public async Task<ActionResult<OrderItemsVM>> CancelOrder(OrderId dto)
        {

            //return await _context.Carts.ToListAsync();
            try
            {
                var orders = _cancelOrderIdDapperRepository.CancelOrder(dto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/GetCancelOrder")]
        public async Task<ActionResult<OrderItemsVM>> GetCancelOrder(GetOrdersListId dto)
        {
            //return await _context.Carts.ToListAsync();
            try
            {
                var orders = _orderCancelDapperRepository.OrderCancelLoad(dto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/GetFinalOrder")]
        public async Task<ActionResult<OrderItemsVM>> GetFinalOrder(GetOrdersListId dto)
        {

            //return await _context.Carts.ToListAsync();
            try
            {
                var orders = _orderFinalDapperRepository.OrderFinalLoad(dto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤訊息: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/RetornOrder")]
        public async Task<ActionResult> RetornOrder(OrderReturnDto dto)
        {

            TimeSpan timeDifference = (TimeSpan)(DateTime.Now - dto.orderDateTime);

            if (timeDifference.TotalDays <= 7)
            {
                try
                {
                    _orderReturnDapperRepository.OrderReturnEdit(dto);
                    return Ok("已更新退貨狀態");
                }
                catch (Exception ex)
                {
                    return BadRequest($"錯誤訊息: {ex.Message}");
                }
            }
            else
            {
                return BadRequest($"錯誤訊息:大於7天不能退款");
            }

        }

        [HttpPost]
        [Route("/RetornOrderMailSent")]
        public async Task<ActionResult> RetornOrderMailSent([FromBody] UserDto dto)
        {
            try
            {
                string from = "yoyoann2023@gmail.com";
                string to = dto.email;
                string subject = "布可網路書店退貨通知";
                string body = $"親愛的{dto.name}您好，已收到您的退貨申請，請把到貨之商品寄回\"桃園市中壢區新生路二段421號\"，寄件人請填上商品收件人姓名以利核銷，在收到您寄回的商品後會盡速完成退款作業，感謝您的配合";
                var emailHelperInstance = new EmailHelper();


                emailHelperInstance.SendFromGmail(from, to, subject, body);
                return Ok("退貨信件已寄出");
            }
            catch (Exception ex)  
            {
                return BadRequest($"退貨信件發送失敗: {ex.Message}");  
            }
        }

        [HttpPost]
        [Route("/CheckOrderToFinal")]
        public async Task<ActionResult> CheckOrderToFinal(IEnumerable<OrderReturnDto> dto)
        {
            foreach (var item in dto)
            {
                if (item.orderDateTime != null)
                {
                    TimeSpan timeDifference = (TimeSpan)(DateTime.Now - item.orderDateTime);

                    if (timeDifference.TotalDays > 7)
                    {
                        try
                        {
                            _orderReturnDapperRepository.CheckOrderToFinal(item);
                            return Ok("更新已完成狀態");
                        }
                        catch (Exception ex)
                        {
                            return BadRequest($"錯誤訊息: {ex.Message}");
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return Ok("更新已完成狀態");
        }

    }
}
