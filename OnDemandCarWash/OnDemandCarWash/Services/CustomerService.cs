//using Microsoft.AspNetCore.Mvc;
//using OnDemandCarWash.Dtos;
//using OnDemandCarWash.Models;
//using OnDemandCarWash.Repositories;

//namespace OnDemandCarWash.Services
//{
//    public class CustomerService
//    {
//        private readonly ICustomerRepository _repo;
//        public CustomerService(ICustomerRepository repo)
//        {
//            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
//        }
//        public async Task<IEnumerable<WashType>> OurServices()
//        {
//            return await _repo.OurServices();
//        }
//        public async Task<ActionResult<string>> GetCoupen(PromocodeDto request)
//        {
//            return await _repo.GetCoupen(request);
//        }
//        public async Task<ActionResult<Order>> StoreOrderDetail(OrderDto request)
//        {

            
//            return await _repo.StoreOrderDetail(request);
            
//        }
//    }
//}
