using Microsoft.EntityFrameworkCore;
using Reservation.Data.Entities;

namespace Reservation.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }
        
        public DbSet<BankCard> BankCards { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Reserving> Reservings { get; set; }

        public DbSet<ServiceMember> ServiceMembers { get; set; }

        public DbSet<ServiceMemberBranch> ServiceMemberBranches { get; set; }

        public DbSet<Payment> PaymentDatas { get; set; }
    }
}
