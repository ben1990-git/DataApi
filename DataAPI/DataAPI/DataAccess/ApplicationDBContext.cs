using DataAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.DataAccess
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Data> Entrys { get; set; }
    }
}
