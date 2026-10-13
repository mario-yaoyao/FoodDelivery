using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Models.DTOs.Requests;
using FoodDelivery.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResDto<object>>> CreateOrder([FromBody] CreateOrderReqDto request)
        {
            try
            {
                var userId = 1;
                var success = await orderService.CreateOrderAsync(userId, request);

                if (!success)
                {
                    return BadRequest(new ApiResDto<object>
                    {
                        Success = false,
                        ErrorMessage = "Failed to create order"
                    });
                }

                return Ok(new ApiResDto<object>
                    {
                        Success = true
                    });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResDto<object>
                {
                    Success = false,
                    ErrorMessage = "An error occurred while creating the order."
                });
            }
        }

        [HttpPost("{orderId}/ready")]
        public async Task<ActionResult<ApiResDto<object>>> MarkOrderReady(int orderId)
        {
            try
            {
                var success = await orderService.MarkOrderReadyAsync(orderId);

                if (!success) return NotFound(new ApiResDto<object>
                {
                    Success = false,
                    ErrorMessage = "Order not found."
                });

                return Ok(new ApiResDto<object>
                {
                    Success = true
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResDto<object>
                {
                    Success = false,
                    ErrorMessage = "An error occurred while marking the order as ready."
                });
            }
        }

        [HttpPost("{orderId}/payment")]
        public async Task<ActionResult<ApiResDto<object>>> ProcessPayment(int orderId)
        {
            try
            {
                var success = await orderService.ProcessPaymentAsync(orderId);

                if (!success) return NotFound(new ApiResDto<object>
                {
                    Success = false,
                    ErrorMessage = "Order not found."
                });

                return Ok(new ApiResDto<object>
                {
                    Success = true
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResDto<object>
                {
                    Success = false,
                    ErrorMessage = "An error occurred while processing the payment."
                });
            }
        }
    }
}
