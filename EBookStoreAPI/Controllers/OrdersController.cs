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

namespace EBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderNotPayDapperRepository _orderNotPayDapperRepository;

        public OrdersController(OrderNotPayDapperRepository orderNotPayDapperRepository)
        {
            _orderNotPayDapperRepository = orderNotPayDapperRepository;
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
    }
}
