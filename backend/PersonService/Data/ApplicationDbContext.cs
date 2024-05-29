using PersonService.Models;
using Microsoft.EntityFrameworkCore;
using PersonService.Dtos;

namespace PersonService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<AccountDto>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<TransactionDto>();
        }
    }
}
