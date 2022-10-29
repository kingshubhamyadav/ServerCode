﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnDemandCarWash.Context;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using Org.BouncyCastle.Bcpg;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnDemandCarWash.Controllers
{
    enum UserStatus
    {
        Inactive,
        Active
    }
    enum Role
    {
        Customer,
        Washer,
        Admin
    }
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        //context
        private CarWashDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(CarWashDbContext context, IConfiguration configuration)//constructor for getting appseeting token string IConfiguration
        {
            _context = context;
            _configuration = configuration;
        }


        //Register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
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
                user.role = Role.Customer.ToString();
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

        //register
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (users == null)
            {
                return BadRequest("User not found");
            }
            if (!VerifyPasswordHash(request.Password, users.PasswordHash, users.PasswordSalt))
            {
                return BadRequest("wrong password");
            }

            // token will be passed
            string token = CreateToken(users);
            return Ok(token);
        }

        //check password is correct or not
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        // Create Token 
        private string CreateToken(User user)
        {
            string userId = user.userId.ToString();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.role),
                new Claim(ClaimTypes.Email,user.email),
                new Claim(ClaimTypes.MobilePhone,user.phone),
                new Claim(ClaimTypes.UserData,userId),
            };
            //install this package- Microsoft.IdentityModel.Tokens;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            // add credientials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //create token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
                );

            // string we want form the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
