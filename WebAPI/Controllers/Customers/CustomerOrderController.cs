using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/Order")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerOrderController(IOrderService orderService, IMapper mapper, ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet("ByRestaurant/{RestaurantId}")]
        public async Task<IActionResult> GetOrdersByCustomer(long RestaurantId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);

            long customerId = customers.FirstOrDefault().Id;

            return Ok(new SuccessResponse<object>("",
                new
                {
                    Ongoing = _mapper.Map<IEnumerable<OrderCardDetailsDTO>>(await _orderService.GetOnGoingOrdersByUserAsync(customerId, RestaurantId)).OrderByDescending(x => x.Id),
                    Past = _mapper.Map<IEnumerable<OrderCardDetailsDTO>>(await _orderService.GetPastOrdersByUserAsync(customerId, RestaurantId)).OrderByDescending(x => x.Id)
                }));
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long Id)
        {
            return Ok(new SuccessResponse<IEnumerable<OrderDTO>>("", _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(Id))));
        }
        [HttpGet("{Id}/Refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshOrderDetails(long Id)
        {
            IEnumerable<OrderDTO> result = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(Id));
            return Ok(new SuccessResponse<object>("", new { result.FirstOrDefault().Status, result.FirstOrDefault().EstimatedDeliveryMinutes }));
        }

        [HttpGet("ByOrderNumber/{OrderNumber}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByOrderNo(string OrderNumber)
        {
            return Ok(new SuccessResponse<IEnumerable<OrderDTO>>("", _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByOrderNoAsync(OrderNumber))));
        }

        [HttpPut("Cancel")]
        public async Task<IActionResult> CancelOrderById(OrderCancellationDTO Model)
        {
            IEnumerable<Order> Orders = await _orderService.GetOrderByIdAsync(Model.OrderId);

            Orders.FirstOrDefault().Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
            Orders.FirstOrDefault().CancelationReason = Model.CancellationReason;

            await _orderService.UpdateOrderAsync(Orders.FirstOrDefault());

            return Ok(new SuccessResponse<string>("Order canceled successfully", ""));
        }

    }
}
