using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportFriends.API.Models;

namespace SportFriends.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options){}
        
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}