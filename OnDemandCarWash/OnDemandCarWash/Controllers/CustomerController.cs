using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using System.Data;

namespace OnDemandCarWash.Controllers
{
    enum CoupenStatus
    {
        Inactive,
        Active
    }
    enum PaymentStatus
    {
        Fail,
        Success
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {

        private CarWashDbContext _context;

        public CustomerController(CarWashDbContext context)
        {
            _context = context;
        }

        [HttpGet("OurServices")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<WashTypeDto>> OurServices()
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
        [Authorize(Roles = "Customer")]
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
                        return BadRequest("Invalid Coupon");
                    }
                    return BadRequest("Invalid Coupon");
                }
                return BadRequest("Coupon Have Been Used Before");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }



        //stores all the details after payment (order table details + paymentDetais table)
        [HttpPost("StoreOrderDetail")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> StoreOrderDetail(OrderDto request)
        {
            var order = new Order();

            order.timeOfWash = request.timeOfWash;
            order.dateOfWash = request.dateOfWash;
            order.location = request.location;
            order.userId = request.userId;
            order.washerUserId = -1;
            order.rating = "null";
            order.code = request.code;//promocode
            order.orderStatus = OrderStatus.PENDING.ToString();
            order.timeStamp = DateTime.Now.ToString();
            order.washTypeId = request.washTypeId;

            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                var orderId = _context.Orders.Where(x => x.timeStamp == order.timeStamp && x.dateOfWash == order.dateOfWash && x.timeOfWash == order.timeOfWash && x.userId == order.userId).SingleOrDefault().orderId;
                
                // Save Car Details
                if (!SaveCarDetail(request.carNumber, request.carType, request.carImg, orderId))
                {
                    return BadRequest("Something went wrong in car details");
                }
                
                // Save payment details
                if (!SavePaymentDetail(request.amountPaid, request.totalDiscount, orderId))
                {
                    return BadRequest("Something went wrong in payment");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred at StoreOrderDetail in CustomerRepo");
                return BadRequest(ex);
            }

        }
        //saving payment details 
        private bool SavePaymentDetail(string amountPaid, string totalDiscount, int orderId)
        {
            var pay = new PaymentDetail();
            pay.orderId = orderId;
            pay.amountPaid = amountPaid;
            pay.totalDiscount = totalDiscount;
            pay.paymentStatus = PaymentStatus.Success.ToString();
            pay.timeStamp = DateTime.Now.ToString();
            try
            {
                _context.PaymentDetails.AddAsync(pay);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) //make custom exception
            {
                Console.WriteLine("Error in payment details in Checkout");
                return false;
            }
        }
        // saving car details
        private bool SaveCarDetail(string carNumber, string carType, string carImg, int orderId)
        {
            var car = new CarDetail();
            car.orderId = orderId;
            car.carNumber = carNumber;
            car.carType = carType;
            car.carImg = carImg;
            car.timeStamp = DateTime.Now.ToString();
            try
            {
                _context.carDetails.AddAsync(car);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) //make custom exception
            {
                Console.WriteLine("Error in adding car details");
                return false;
            }
        }

        [HttpGet("OrderHistory/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IEnumerable<OrderHistoryDto>> OrderHistory(int id)
        {
            try
            {
                var orders = await _context.Orders.Where(x => x.userId == id)
                   .Select(p => new OrderHistoryDto()
                   {
                       timeOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).timeOfWash,
                       dateOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).dateOfWash,
                       location = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).location,
                       code = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).code,
                       amountPaid = _context.PaymentDetails.SingleOrDefault(x => x.orderId == p.orderId).amountPaid,
                       totalDiscount = _context.PaymentDetails.SingleOrDefault(x => x.orderId == p.orderId).totalDiscount
                   }).ToListAsync();

                if (orders == null)
                {
                    return null;
                }
                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetWasher")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderDto>> GetWasher()
        {
            try
            {
                var washer = await _context.Users.Where(x => x.role == Role.Washer.ToString()).ToListAsync();

                if (washer == null)
                {
                    return null;
                }
                return Ok(washer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit-profile/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<CustomerProfileDto>> UpdateWasherAsync(int id, CustomerProfileDto customer)
        {
            try
            {
                //var user = await _context.Users.FirstOrDefaultAsync(x => x.userId == id);
                _context.Users.SingleOrDefault(x => x.userId == customer.userId).firstName = customer.FirstName;
                _context.Users.SingleOrDefault(x => x.userId == customer.userId).lastName = customer.LastName;
                _context.Users.SingleOrDefault(x => x.userId == customer.userId).email = customer.Email;
                _context.Users.SingleOrDefault(x => x.userId == id).phone = customer.Phone;

                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at UpdateCustomer in CustomerRepo");
                return null;
            }
            finally
            {

            }

        }

        [HttpGet("view-profile/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<CustomerProfileDto>> GetCustomerProfileAsync(int id)
        {
            try
            {
                var user = await _context.Users.Where(x => x.userId == id)
                    .Select(p => new CustomerProfileDto()
                    {
                        userId = _context.Users.SingleOrDefault(x => x.userId == p.userId).userId,
                        FirstName = _context.Users.SingleOrDefault(x => x.userId == p.userId).firstName,
                        LastName = _context.Users.SingleOrDefault(x => x.userId == p.userId).lastName,
                        Email = _context.Users.SingleOrDefault(x => x.userId == p.userId).email,
                        Phone = _context.Users.SingleOrDefault(x => x.userId == p.userId).phone,
                        Img = _context.Users.SingleOrDefault(x => x.userId == p.userId).img
                    }).SingleOrDefaultAsync();
                if (user == null)
                {
                    return BadRequest("Unable to fetch user.");
                }
                return Ok(user);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occurred at GetCustomer in CustomerRepo");
                return null;
            }
            finally
            {

            }
        }

        [HttpPost("upload-profile-img")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<User>> ProfileImageUploadAsync(ImageDto request)
        {
            try
            {
                var img = await _context.Users.Where(x => x.userId == request.userId).SingleOrDefaultAsync();
                img.img = request.img;
                await _context.SaveChangesAsync();
                return img;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error occurred at ProfileImageUpload in CutomerRepo");
                return null;
            }
            finally
            {

            }
        }
    }
}
