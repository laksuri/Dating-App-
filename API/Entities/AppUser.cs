using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public string UserName{get;set;}
        public int ID{get;set;}
        public byte[] PasswordHash { get; set; }
        public byte[]  PasswordSalt {get;set;}
    }
}