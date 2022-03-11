using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MyAdminpro.Models
{
    public class MSDBContext:DbContext
    {
       public MSDBContext() : base() { }

        public DbSet<Tbl_Users> Tbl_Users { get; set; }
        public DbSet<VerifyAccount> verifyAccounts { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConnection.ConnectionStr);
        }
    }
}
