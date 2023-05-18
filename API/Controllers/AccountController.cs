using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using API.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context=context;
            _tokenService=tokenService;
        }
        //Register New User to the Application
        [HttpPost("register")] //Eg : api/account/register
        public async Task<ActionResult<UserDTO>> RegisterUser(RegisterDTO registerUser)
        {
            //Check if username already exists in DB
            var entity=await _context.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower()==registerUser.UserName.ToLower());
            if(entity!=null){ return Unauthorized("User Already Exists");}
            using var hmac=new HMACSHA512();
            
            var user=new AppUser()
            {
                UserName=registerUser.UserName,
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password)),
                PasswordSalt=hmac.Key
            };
            _context.Users.Add(user);
            await  _context.SaveChangesAsync();
            

            return new UserDTO{
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user)
            };            
        }
        //Login to the application with credentials.
        [HttpPost("Login")] // Eg: api/account/login
        public async Task<ActionResult<UserDTO>> LoginUser (LoginDTO loginUser)
        {
            var user= await _context.Users.SingleOrDefaultAsync(x=>x.UserName.ToLower()==loginUser.UserName.ToLower());
            if(user==null){ return Unauthorized("User does not exist");}
             
            using var hmac=new HMACSHA512(user.PasswordSalt);
            var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password));
            for(int i=0;i<computedHash.Length;i++)
            {
                if(user.PasswordHash[i]!=computedHash[i]){return Unauthorized("Invalid Credentials");}
            }
            return new UserDTO(){
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }
    
    }
}