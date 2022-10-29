using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection.Metadata.Ecma335;

namespace OnDemandCarWash.Repositories
{
    public class WasherRepository : IWasherRepository
    {
        private readonly CarWashDbContext _context; //dbcontext
        private readonly IMapper _mapper;
        public WasherRepository(CarWashDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region GetWasherMethod
        public async Task<ActionResult<WasherProfileDto>> GetWasherAsync(int id)
        {
            try
            {
                //var user = await _context.Users.FirstOrDefaultAsync(user => user.userId == id); //assuming status of user is checked on login
                var user = await _context.Users.Where(x => x.userId == id)
                    .Select(p => new WasherProfileDto()
                    {
                        userId = _context.Users.SingleOrDefault(x => x.userId == p.userId).userId,
                        FirstName = _context.Users.SingleOrDefault(x => x.userId == p.userId).firstName,
                        LastName = _context.Users.SingleOrDefault(x => x.userId == p.userId).lastName,
                        Email = _context.Users.SingleOrDefault(x => x.userId == p.userId).email,
                        Phone = _context.Users.SingleOrDefault(x => x.userId == p.userId).phone,
                        //Pincode = _context.Address.SingleOrDefault(x => x.userId == p.userId).pincode,
                        //City = _context.Address.SingleOrDefault(x => x.userId == p.userId).city,
                        //State = _context.Address.SingleOrDefault(x => x.userId == p.userId).state,
                        Img = _context.Users.SingleOrDefault(x => x.userId == p.userId).img
                        
                    }).SingleOrDefaultAsync();
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at GetWasher in WasherRepo");
                return null;
            }
            finally
            {

            }

        }
        #endregion

        #region UpdateWasherMethod
        public async Task<ActionResult<WasherProfileDto>> UpdateWasherAsync(int id, WasherProfileDto washer)
        {
            try
            {
                //var user = await _context.Users.FirstOrDefaultAsync(x => x.userId == id);
                _context.Users.SingleOrDefault(x => x.userId == washer.userId).firstName = washer.FirstName;
                _context.Users.SingleOrDefault(x => x.userId == washer.userId).lastName = washer.LastName;
                _context.Users.SingleOrDefault(x => x.userId == washer.userId).email = washer.Email;
                _context.Users.SingleOrDefault(x => x.userId == washer.userId).phone = washer.Phone;
                //_context.Address.SingleOrDefault(x => x.userId == washer.userId).pincode = washer.Pincode;
                //_context.Address.SingleOrDefault(x => x.userId == washer.userId).city = washer.City;
                //_context.Address.SingleOrDefault(x => x.userId == washer.userId).state = washer.State;

                await _context.SaveChangesAsync();
                return washer;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at UpdateWasher in WasherRepo");
                return null;
            }
            finally
            {

            }

        }
        #endregion

        #region CheckWasherExistMethod
        public async Task<bool> WasherExistsAsync(int id)
        {
            try
            {
                return await _context.Users.AnyAsync(user => user.userId == id); //returns true or false
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at WasherExists in WasherRepo");
                return false;
            }
            finally
            {

            }
        }
        #endregion

        #region GetWasherRequestsMethod
        public async Task<IEnumerable<WasherRequestsDto>> GetWasherRequestsAsync()
        {
            try
            {
                //var requests = await _context.Orders.Where(x => x.orderStatus == "PENDING").ToListAsync();
                var requests = await _context.Orders.Where(x => x.orderStatus == "PENDING")
                    .Select(p => new WasherRequestsDto()
                    {
                        orderId = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).orderId,
                        customerId = _context.Users.SingleOrDefault(x => x.userId == p.userId).userId,
                        firstName = _context.Users.SingleOrDefault(x => x.userId == p.userId).firstName,
                        lastName = _context.Users.SingleOrDefault(x => x.userId == p.userId).lastName,
                        email = _context.Users.SingleOrDefault(x => x.userId == p.userId).email,
                        timeOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).timeOfWash,
                        dateOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).dateOfWash,
                        location = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).location,
                        category = _context.washTypes.SingleOrDefault(x => x.washTypeId == p.washTypeId).categories
                    }).ToListAsync();
                if (requests == null)
                {
                    return null;
                }
                return requests;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at WasherRequests in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region WashCompleteMethod
        public async Task<ActionResult<AfterWash>> AddAfterWashAsync(AfterWashDto request)
        {
            try
            {
                var afterwash = new AfterWash();
                afterwash.orderId = request.orderId;
                afterwash.waterUsed = request.waterUsed;
                afterwash.carImg = request.carImg;
                afterwash.timeStamp = DateTime.Now.ToString();

                _context.afterWashes.Add(afterwash);

                //once we add the after wash details for the invoice we change that orders status from IN-PROGRESS to COMPLETED.
                _context.Orders.Where(x => x.orderId == afterwash.orderId).SingleOrDefault().orderStatus = "COMPLETED";
                _context.Orders.Where(x => x.orderId == afterwash.orderId).SingleOrDefault().rating = request.rating;
                await _context.SaveChangesAsync();
                return afterwash;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error occurred at AddAfterWash in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region CurrentOrdersMethod
        public async Task<IEnumerable<WasherRequestsDto>> GetCurrentOrdersAsync(int id)
        {
            try
            {
                //var orders = await _context.Orders.Where(x => x.orderStatus == "ACCEPTED" || x.orderStatus == "IN-PROGRESS").ToListAsync();
                var orders = await _context.Orders.Where(x => (x.orderStatus == "ACCEPTED" || x.orderStatus == "IN-PROGRESS") && x.washerUserId == id)
                    .Select(p => new WasherRequestsDto()
                    {
                        orderId = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).orderId,
                        firstName = _context.Users.SingleOrDefault(x => x.userId == p.userId).firstName,
                        lastName = _context.Users.SingleOrDefault(x => x.userId == p.userId).lastName,
                        timeOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).timeOfWash,
                        dateOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).dateOfWash,
                        location = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).location,
                        category = _context.washTypes.SingleOrDefault(x => x.washTypeId == p.washTypeId).categories
                    }).ToListAsync();
                if (orders == null)
                {
                    return null;
                }
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at CurrentOrders in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region PastOrdersMethod
        public async Task<IEnumerable<WasherRequestsDto>> GetPastOrdersAsync(int id)
        {
            try
            {
                //var orders = await _context.Orders.Where(x => x.orderStatus == "CANCELLED" || x.orderStatus == "COMPLETED").ToListAsync();
                var orders = await _context.Orders.Where(x => (x.orderStatus == "CANCELLED" || x.orderStatus == "COMPLETED") && x.washerUserId == id)
                    .Select(p => new WasherRequestsDto()
                    {
                        orderId = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).orderId,
                        firstName = _context.Users.SingleOrDefault(x => x.userId == p.userId).firstName,
                        lastName = _context.Users.SingleOrDefault(x => x.userId == p.userId).lastName,
                        timeOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).timeOfWash,
                        dateOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).dateOfWash,
                        location = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).location,
                        category = _context.washTypes.SingleOrDefault(x => x.washTypeId == p.washTypeId).categories
                    }).ToListAsync();
                if (orders == null)
                {
                    return null;
                }
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at PastOrders in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region InvoiceDetailsMethod
        public async Task<IEnumerable<SendInvoiceDto>> GetInvoiceDetailsAsync(int id)
        {
            try
            {
                //var invoice = await _context.Orders.Where(x => x.orderStatus == "IN-PROGRESS").ToListAsync();
                var invoices = await _context.Orders.Where(x => x.orderStatus == "IN-PROGRESS" && x.washerUserId == id)
                    .Select(p => new SendInvoiceDto()
                    {
                        orderId = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).orderId,
                        firstName = _context.Users.SingleOrDefault(x => x.userId == p.userId).firstName,
                        lastName = _context.Users.SingleOrDefault(x => x.userId == p.userId).lastName,
                        timeOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).timeOfWash,
                        dateOfWash = _context.Orders.SingleOrDefault(x => x.orderId == p.orderId).dateOfWash,
                        category = _context.washTypes.SingleOrDefault(x => x.washTypeId == p.washTypeId).categories
                    }).ToListAsync();

                if (invoices.Count < 0)
                {
                    return null;
                }
                return invoices;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at InvoiceDetails in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region AcceptRequestMethod
        public async Task<ActionResult<Order>> AcceptRequestAsync(AcceptRequestDto request)
        {
            try
            {
                var order = await _context.Orders.Where(x => x.orderId == request.orderId).SingleOrDefaultAsync();
                order.washerUserId = request.washerId;
                order.orderStatus = "IN-PROGRESS";
                await _context.SaveChangesAsync();
                return order;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error occurred at AcceptRequest in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region ProfileImageUploadMethod
        public async Task<ActionResult<User>> ProfileImageUploadAsync(ImageDto request)
        {
            try
            {
                var img = await _context.Users.Where(x => x.userId == request.userId).SingleOrDefaultAsync();
                img.img = request.img;
                await _context.SaveChangesAsync();
                return img;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error occurred at ProfileImageUpload in WasherRepo");
                return null;
            }
            finally
            {

            }
        }
        #endregion
    }
}
