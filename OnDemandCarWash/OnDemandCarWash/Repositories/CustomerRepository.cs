//using Microsoft.AspNetCore.Mvc;

//using Microsoft.EntityFrameworkCore;
//using OnDemandCarWash.Context;
//using OnDemandCarWash.Controllers;
//using OnDemandCarWash.Dtos;
//using OnDemandCarWash.Models;

//namespace OnDemandCarWash.Repositories
//{
//    public class CustomerRepository : ICustomerRepository
//    {
//        private CarWashDbContext _context;

//        public CustomerRepository(CarWashDbContext context)
//        {
//            _context = context;
//        }


//        public async Task<IEnumerable<WashType>> OurServices()
//        {
//            try
//            {
//                var ourServices = await _context.washTypes.ToListAsync();
//                if (ourServices == null)
//                {
//                    return null;
//                }
//                return ourServices;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }

//            return null;


//        }
//        //check whether the promocode is valid or not and return discount value

//        public async Task<ActionResult<string>> GetCoupen(PromocodeDto request)
//        {
//            try
//            {
//                var coupenUsed = await _context.Orders.Where(x => x.userId == 1).FirstOrDefaultAsync(x => x.code == request.code);//take user from header token
//                if (coupenUsed == null)// coupen have not been used before
//                {
//                    var coupen = await _context.Promocodes.FirstOrDefaultAsync(x => x.code == request.code);
//                    if (coupen != null)
//                    {
//                        if (coupen.status == CoupenStatus.Active.ToString())
//                        {
//                            return coupen.discount;
//                        }
//                        Console.WriteLine("Expired Coupen");
//                        ; return null;
//                    }
//                    Console.WriteLine("Invalid Coupen");
//                    return null;
//                }
//                Console.WriteLine("Coupen Have Been Used Before");
//                return null;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }

//        }





//        //stores all the details after payment (order table details + paymentDetais table)
        
//        public async Task<ActionResult<Order>> StoreOrderDetail(OrderDto request)
//        {
//            var order = new Order();

//            order.timeOfWash = request.timeOfWash;
//            order.dateOfWash = request.dateOfWash;
//            order.orderStatus = OrderStatus.InProgress.ToString();
//            order.location = request.location;
//            order.userId = request.userId; // header token
//            order.washerUserId = request.washerUserId;
//            order.rating = "null";
//            order.code = request.code;//promocode



//            try
//            {
//                await _context.Orders.AddAsync(order);
//                await _context.SaveChangesAsync();

//                //Save Car Details
//                string img = "null";// take img from checkout page
//                if (!SaveCarDetail(request.carNumber, request.carType, img, order.orderId))
//                {
//                    Console.WriteLine("Something went wrong in entring car details");
//                    return null;
//                }
//                // Save payment details
//                if (!SavePaymentDetail(request.amountPaid, request.totalDiscount, order.orderId))
//                {
//                    Console.WriteLine("Something went wrong in payment");
//                    return null;
//                }
//                return order;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }

//        }
//        private bool SavePaymentDetail(string amountPaid, string totalDiscount, int orderId)
//        {
//            var pay = new PaymentDetail();
//            pay.orderId = orderId;
//            pay.amountPaid = amountPaid;
//            pay.totalDiscount = totalDiscount;
//            pay.paymentStatus = PaymentStatus.Success.ToString();
//            try
//            {
//                _context.PaymentDetails.AddAsync(pay);
//                _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex) //make custom exception
//            {
//                return false;
//            }
//        }
//        private bool SaveCarDetail(string carNumber, string carType, string img, int orderId)
//        {
//            var car = new CarDetail();
//            car.orderId = orderId;
//            car.carNumber = carNumber;
//            car.carType = carType;
//            car.carImg = img;
//            try
//            {
//                _context.carDetails.AddAsync(car);
//                _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex) //make custom exception
//            {
//                return false;
//            }
//        }

//        [HttpGet("OrderHistory")]
//        public async Task<ActionResult<OrderHistoryDto>> OrderHistory(int userId)
//        {

//            try
//            {

//                var orders = await _context.Orders.Where(x => x.userId == userId)
//                     .Select(p => new OrderHistoryDto()
//                     {
//                         timeOfWash = _context.Orders.SingleOrDefault(x => x.userId == p.userId).timeOfWash,
//                         dateOfWash = _context.Orders.SingleOrDefault(x => x.userId == p.userId).dateOfWash,
//                         location = _context.Orders.SingleOrDefault(x => x.userId == p.userId).location,

//                         amountPaid = _context.PaymentDetails.SingleOrDefault(x => x.orderId == p.orderId).amountPaid,
//                         totalDiscount = _context.PaymentDetails.SingleOrDefault(x => x.orderId == p.orderId).totalDiscount,
//                     }).ToListAsync();


//                if (orders != null)
//                {
//                    return Ok(orders);
//                }
//                return null;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error occurred at order history");
//                return null;
//            }
//            finally
//            {

//            }

//        }


//    }
//}
