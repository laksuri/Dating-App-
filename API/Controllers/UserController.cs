using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Entities;
using API.Data;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context=context;
        }
        [HttpGet("user/getusers")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }
        [HttpGet("user/getUserById/{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            var users = await _context.Users.FindAsync(id);

            return users;
        }
    }
}