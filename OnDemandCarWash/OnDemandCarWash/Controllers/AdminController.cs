using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace OnDemandCarWash.Controllers
{
    enum PromocodeSatus
    {
        Inactive,
        Active
    }
    enum OrderStatus
    {
        PENDING,
        ACCEPTED,
        IN_PROGRESS,
        COMPLETED
    }
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private CarWashDbContext _context;

        public AdminController(CarWashDbContext context)
        {
            _context = context;
        }

        //create wash type
        [HttpPost("CreateServices")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<WashType>> CreateServices(CreateWashTypeDto request)
        {
            try
            {
                var services = new WashType();
                services.categories = request.catogries;
                services.discription = request.discription;
                services.charges = request.charges;
                await _context.washTypes.AddAsync(services);
                await _context.SaveChangesAsync();
                return Ok(services);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }


        }

        //add promocode
        [HttpPost("CreatePromocode")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Promocode>> CreatePromocode(CreatePromocodeDto request)
        {
            try
            {
                var promo = new Promocode();
                promo.code = request.code.ToUpper();
                promo.discount = request.discount;
                promo.status = PromocodeSatus.Active.ToString();
                promo.timeStamp = DateTime.Now.ToString();
                await _context.Promocodes.AddAsync(promo);
                await _context.SaveChangesAsync();
                return Ok(promo);
            }
            catch
            {
                return BadRequest("problem in saving Promocode");
            }


        }

        //add washer
        [HttpPost("CreateWasher")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> CreateWasher(CreatWasherDto request)
        {

            CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

            var checkUserName = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.userName);
            if (checkUserName == null)
            {
                var user = new User();
                user.Username = request.userName;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.firstName = request.firstName;
                user.lastName = request.lastName;
                user.email = request.email;
                user.phone = request.phone;
                user.role = Role.Washer.ToString();
                user.img = "https://bootdey.com/img/Content/avatar/avatar7.png";
                user.status = UserStatus.Active.ToString();
                user.timeStamp = DateTime.Now.ToString();

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            else
            {
                return BadRequest("User Name allready taken");
            }

        }
        //generate hash code for password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // get all washer
        [HttpGet("GetWasher")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetWasherDto>> GetWasher()
        {
            try
            {
                var washers = await _context.Users.Where(x => x.role == Role.Washer.ToString()).ToListAsync();
                if (washers == null)
                {
                    return BadRequest("Sorry we dont have any washers");
                }
                return Ok(washers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // get all washer
        [HttpGet("GetCustomer")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetWasherDto>> GetCustomer() // GetWasherDto is used because washer and customer have same details to return
        {
            try
            {
                var cust = await _context.Users.Where(x => x.role == Role.Customer.ToString()).ToListAsync();
                if (cust == null)
                {
                    return BadRequest("Sorry we dont have any cust");
                }
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // get all pending order
        [HttpGet("PendingOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDto>> PendingOrder()
        {
            try
            {
                //var pen = await _context.Orders.Where(x => x.orderStatus == OrderStatus.PENDING.ToString()).ToListAsync();
                var pen = await _context.Orders.Where(x => x.orderStatus == "PENDING")
                    .Select(p => new AdminWashRequestDto()
                    {
                        orderId = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).orderId,
                        code = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).code,
                        timeOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).timeOfWash,
                        dateOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).dateOfWash,
                        location = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).location,
                        package = _context.washTypes.SingleOrDefault(x => x.washTypeId == p.washTypeId).categories,
                        userMail = _context.Users.SingleOrDefault(x => x.userId == p.userId).email
                    }).ToListAsync();
                if (pen == null)
                {
                    return BadRequest("Sorry we dont have any pending order");
                }
                return Ok(pen);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // get all  order
        [HttpGet("AllOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDto>> AllOrder()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                if (orders == null)
                {
                    return BadRequest("Sorry we dont have any order");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // get all promocode
        [HttpGet("AllPromocode")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PromocodeDto>> AllPromocode()
        {
            try
            {
                var promo = await _context.Promocodes.ToListAsync();
                if (promo == null)
                {
                    return BadRequest("Sorry we dont have any order");
                }
                return Ok(promo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        //assign washer to order
        [HttpPost("accept-request")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AcceptRequestAsync(AdminAcceptRequestDto request)
        {
            try
            {
                var order = await _context.Orders.Where(x => x.orderId == request.orderId).SingleOrDefaultAsync();
                var id = await _context.Users.Where(x => x.Username == request.washerId)
                    .Select(p => new GetCustomerIdDto()
                    {
                        userId = _context.Users.SingleOrDefault(x => x.userId == p.userId).userId
                    }).SingleOrDefaultAsync();
                order.washerUserId = id.userId;
                order.orderStatus = "IN-PROGRESS";
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured at AcceptRequest in AdminRepo");
                return BadRequest(ex);
            }
            finally
            {

            }

        }

        //gets customerID from username
        [HttpGet("get-customer-id/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetCustomerIdDto>> GetUserId(string name)
        {
            try
            {
                var customerId = await _context.Users.Where(x => x.Username == name)
                    .Select(p => new GetCustomerIdDto()
                    {
                        userId = _context.Users.SingleOrDefault(x => x.userId == p.userId).userId
                    }).SingleOrDefaultAsync();

                return Ok(customerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured at GetUserId in AdminRepo");
                return BadRequest(ex);
            }
            finally
            {

            }

        }
    }
}
