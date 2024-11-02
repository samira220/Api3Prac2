using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.DbContextApi
{
    public class TestApiDb: DbContext
    {
        public TestApiDb(DbContextOptions options):base(options) 
        {

        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Readers> Readers { get; set; }
        public DbSet<Genres_Books> Genres { get; set; }
        public DbSet<History_Book_Rental> History_Book_Rentals { get; set; }
    }
}
