using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
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
    public class AdminController : ControllerBase
    {
        private CarWashDbContext _context;

        public AdminController(CarWashDbContext context)
        {
            _context = context;
        }

        //create wash type
        [HttpPost("CreateServices")]
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
            catch(Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
          

        }

        //add promocode
        [HttpPost("CreatePromocode")]
        public async Task<ActionResult<Promocode>> CreatePromocode(CreatePromocodeDto request)
        {
            try
            {
                var promo = new Promocode();
                promo.code = request.code;
                promo.discount = request.discount;
                promo.status = PromocodeSatus.Active.ToString();
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
                user.lastName = request.firstName;
                user.email = request.firstName;
                user.phone = request.firstName;
                user.role = Role.Washer.ToString();
                user.img = "";
                user.status = UserStatus.Active.ToString();
                user.timeStamp = "6/6/2022";

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
        public async Task<ActionResult<GetWasherDto>> GetWasher()
        {
            try
            {
                var washers = await _context.Users.Where(x=>x.role==Role.Washer.ToString()).ToListAsync();
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
        public async Task<ActionResult<OrderDto>> PendingOrder()
        {
            try
            {
                var pen = await _context.Orders.Where(x => x.orderStatus == OrderStatus.PENDING.ToString()).ToListAsync();
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




    }
}
