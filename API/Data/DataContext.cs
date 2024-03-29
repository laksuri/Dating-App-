using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;

namespace API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<AppUser> Users {get;set;}
    }
}