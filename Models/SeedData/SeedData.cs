using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TabApp.Enums;

namespace TabApp.Models.SeedData
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new dbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<dbContext>>()))
            {
                context.Database.EnsureCreated();

                initRepairStatusTable(context);

                initPickupCodesTable(context);

                initAdminAccount(context);
            }
        }

        private static void initAdminAccount(dbContext context)
        {
            if (context.Person.Any())
            {
                return;
            }

            var person = new Person
            {
                Name = "Admin",
                Surname = "Admin",
                Address = "Admin",
                Email = "admin@mail.com",
                Role = "Admin",
                PhoneNumber = "000000000",
                LoginCredentials = new LoginCredentials
                {
                    UserName = "admin",
                    Password = "admin"
                }

            };

            var support = new Person
            {
                Name = "Support",
                Surname = "Support",
                Address = "Support",
                Email = "support@mail.com",
                Role = "Support",
                PhoneNumber = "000000000",
                LoginCredentials = new LoginCredentials
                {
                    UserName = "support",
                    Password = "support"
                }

            };

            context.Add(support);
            context.Add(person);
            context.SaveChanges();
        }

        private static void initRepairStatusTable(dbContext context)
        {

            if (context.RepairStatus.Any())
            {
                return;   // DB has been seeded
            }

            context.RepairStatus.AddRange(
                new RepairStatus
                {
                    Status = RepairStatuses.Accepted
                },

                new RepairStatus
                {
                    Status = RepairStatuses.InProgress
                },
                new RepairStatus
                {
                    Status = RepairStatuses.Ready
                },
                new RepairStatus
                {
                    Status = RepairStatuses.Issued
                }
            );
            context.SaveChanges();
        }

        private static void initPickupCodesTable(dbContext context)
        {

            if (context.PickupCodes.Any())
            {
                return;   // DB has been seeded
            }

            for (int i = 0; i < 10000; i++)
            {
                context.Add(new PickupCode { Value = i.ToString("D4") });
            }

            context.SaveChanges();
        }
    }
}
