using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IcraTakipProgrami.Models
{
    public class IdentityContext:IdentityDbContext<IdentityUser,AppRole,string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { 

        }
    }
}
