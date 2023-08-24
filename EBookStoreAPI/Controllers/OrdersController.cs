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
        

        public OrdersController(OrderNotPayDapperRepository orderNotPayDapperRepository, OrderItemsDapperRepository orderItemsDapperRepository, OrderPayDapperRepository orderPayDapperRepository, CancelOrderIdDapperRepository cancelOrderIdDapperRepository)
        {
            _orderNotPayDapperRepository = orderNotPayDapperRepository;
            _orderItemsDapperRepository = orderItemsDapperRepository;
            _orderPayDapperRepository = orderPayDapperRepository;
            _cancelOrderIdDapperRepository = cancelOrderIdDapperRepository;
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

    }
}
