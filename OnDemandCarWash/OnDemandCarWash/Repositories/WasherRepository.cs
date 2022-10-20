using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
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
        public async Task<ActionResult<WasherProfileDto>> GetWasherAsync(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.userId == id); //assuming status of user is checked on login
                if (user != null)
                {
                    return _mapper.Map<WasherProfileDto>(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at GetWasher in WasherService");
                return null;
            }
            finally
            {

            }

        }

        public async Task<ActionResult<User>> UpdateWasherAsync(int id, WasherProfileDto washer)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.userId == id);
                if (user == null)
                {
                    return null;
                }
                _mapper.Map(washer, user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at UpdateWasher in WasherService");
                return null;
            }
            finally
            {

            }

        }

        public async Task<bool> WasherExistsAsync(int id)
        {
            try
            {
                return await _context.Users.AnyAsync(user => user.userId == id); //returns true or false
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred at WasherExists in WasherService");
                return false;
            }
            finally
            {

            }
        }
    }
}
