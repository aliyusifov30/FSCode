using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string TGId { get; set; }
        public string TGToken { get; set; }
    }
}
