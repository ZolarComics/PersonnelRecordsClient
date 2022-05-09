using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PersonnelRecordsClient.Views.Pages;

namespace PersonnelRecordsClient.MVVM
{
    class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public ApplicationContext() : base("DefaultConnection") { }

    }
}
