using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Spelers_API.Data
{
    public class OtherContext : IdentityDbContext
    {
        public OtherContext(DbContextOptions<OtherContext> options)
            : base(options)
        {
        }
    }
}
