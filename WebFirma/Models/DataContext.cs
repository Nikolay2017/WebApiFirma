
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace WebFirma.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UsersHistory> UsersHistory { get; set; }
        public DbSet<Otdel> Otdels { get; set; }
        public DbSet<Ispyt> Ispyts { get; set; }
        public DbSet<Kval> Kvals { get; set; }
        public DataContext()
            :base("DbConnection")
        { }

    }
}
