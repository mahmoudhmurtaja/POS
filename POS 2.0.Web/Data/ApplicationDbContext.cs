using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POS_2._0.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_2._0.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CashSale> CashSales { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategorys { get; set; }
        public DbSet<Premium> Premiums { get; set; }
        public DbSet<PremiumSale> PremiumSales { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
