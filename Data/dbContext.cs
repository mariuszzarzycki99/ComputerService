using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TabApp.Models;

public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Item>().HasMany(x => x.Repair).WithOne(x => x.Item);
        builder.Entity<Person>().HasMany(x => x.SendMessage).WithOne(x => x.Sender);
        builder.Entity<Person>().HasMany(x => x.ReciveMessage).WithOne(x => x.Addressee);
        //builder.Entity<RepairStatus>().HasMany(x => x.Repair).WithOne(x => x.Status);
    }

    public DbSet<TabApp.Models.Worker> Worker { get; set; }

    public DbSet<TabApp.Models.Person> Person { get; set; }

    public DbSet<TabApp.Models.Invoice> Invoice { get; set; }

    public DbSet<TabApp.Models.Item> Item { get; set; }

    public DbSet<TabApp.Models.Message> Message { get; set; }

    public DbSet<TabApp.Models.PriceList> PriceList { get; set; }

    public DbSet<TabApp.Models.Repair> Repair { get; set; }

    public DbSet<TabApp.Models.Service> Service { get; set; }

    public DbSet<TabApp.Models.LoginCredentials> LoginCredentials { get; set; }

    public DbSet<TabApp.Models.RepairStatus> RepairStatus { get; set; }

    public DbSet<TabApp.Models.PickupCode> PickupCodes { get; set; }
}
