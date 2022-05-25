using System.Data.Entity;
//using Microsoft.EntityFrameworkCore;

namespace PersonnelRecordsClient.MVVM
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public ApplicationContext() : base("DefaultConnection") { }

    }
}
