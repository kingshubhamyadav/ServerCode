using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;

namespace OnDemandCarWash.Controllers
{
    enum CoupenStatus
    {
        Inactive,
        Active
    }
    enum OrderStatus
    {
        Rejected,
        InProcess,
        Completed
    }
    enum PaymentStatus
    {
        Fail,
        Success
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
       
        private CarWashDbContext _context;

        public CustomerController(CarWashDbContext context)
        {
            _context = context;
        }

        [HttpGet("OurServices")]
        public async Task<ActionResult> OurServices()
        {
            try
            {
                var ourServices = await _context.washTypes.ToListAsync();
                if (ourServices == null)
                {
                    return BadRequest("Sorry we dont have any services");
                }
                return Ok(ourServices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        //check whether the promocode is valid or not and return discount value
        [HttpPost("CheckPromocode")]
        public async Task<ActionResult<Promocode>> GetCoupen(PromocodeDto request)
        {
            try
            {
                var coupenUsed = await _context.Orders.Where(x => x.userId == 1).FirstOrDefaultAsync(x => x.code == request.code);//take user from header token
                if (coupenUsed == null)// coupen have not been used before
                {
                    var coupen = await _context.Promocodes.FirstOrDefaultAsync(x => x.code == request.code);
                    if (coupen != null)
                    {
                        if (coupen.status == CoupenStatus.Active.ToString())
                        { 
                            return Ok(coupen.discount);
                        }
                        return BadRequest("Invalid Co");
                    }
                    return BadRequest("Invalid Coupen");
                }
                return BadRequest("Coupen Have Been Used Before");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }



        //stores all the details after payment (order table details + paymentDetais table)
        [HttpPost("StoreOrderDetail")]
        public async Task<ActionResult<Order>> StoreOrderDetail(OrderDto request)
        {
            var order = new Order();

            order.timeOfWash = request.timeOfWash;
            order.dateOfWash = request.dateOfWash;
            order.orderStatus = OrderStatus.InProcess.ToString();
            order.location = request.location;
            order.userId = 0;
            order.washerUserId = 0;
            order.rating = "null";
            order.code = request.code;//promocode

           

            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                // Save payment details
                if (!SavePaymentDetail(request.amountPaid, request.totalDiscount,order.orderId))
                {
                    return BadRequest("Something went wrong in payment");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        private bool SavePaymentDetail(string amountPaid, string totalDiscount,int orderId)
        {
            var pay = new PaymentDetail();
            pay.orderId = orderId;
            pay.amountPaid = amountPaid;    
            pay.totalDiscount = totalDiscount;
            pay.paymentStatus = PaymentStatus.Success.ToString();
            try
            {
                _context.PaymentDetails.AddAsync(pay);
                _context.SaveChangesAsync();
                return true;
            }catch(Exception ex) //make custom exception
            {
                return false;
            }
        }
    }
}
