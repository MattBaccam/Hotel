namespace ASPScenicHotel.Migrations
{
    using ASPScenicHotel.Models;
    using DataObjects;
    using LogicLayer;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASPScenicHotel.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ASPScenicHotel.Models.ApplicationDbContext";
        }

        protected override void Seed(ASPScenicHotel.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var employeeManager = new EmployeeManager();
            var positions = employeeManager.GetPositions();
            foreach (var position in positions)
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = position.PositionTitle });
            }
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Guest" });

            const string admin = "admin@company.com";
            const string adminPassword = "P@ssw0rd";
            var ogAdmin = employeeManager.GetEmployeeByEmail(admin);
            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = ogAdmin.Email,
                    Email = ogAdmin.Email,
                    FirstName = ogAdmin.FirstName,
                    LastName = ogAdmin.LastName,
                    AppID = ogAdmin.EmployeeID,
                    PositionTitle = "Admin"
                }; 
                
                var guestList = new List<ApplicationUser>();

                guestList.Add(new ApplicationUser()
                {
                    UserName = "jsmith@email.com",
                    Email = "jsmith@email.com",
                    PhoneNumber = "123-456-7890",
                    FirstName = "John",
                    LastName = "Smith",
                    AppID = 100000,
                    PositionTitle = "Guest"
                });

                guestList.Add(new ApplicationUser()
                {
                    UserName = "mjones@email.com",
                    Email = "mjones@email.com",
                    PhoneNumber = "123-456-7890",
                    FirstName = "Mary",
                    LastName = "Jones",
                    AppID = 100001,
                    PositionTitle = "Guest"
                });

                guestList.Add(new ApplicationUser()
                {
                    UserName = "bwilliams@email.com",
                    Email = "bwilliams@email.com",
                    PhoneNumber = "345-678-9012",
                    FirstName = "Bob",
                    LastName = "Williams",
                    AppID = 100002,
                    PositionTitle = "Guest"
                });

                guestList.Add(new ApplicationUser()
                {
                    UserName = "matthewbaccam@email.com",
                    Email = "matthewbaccam@email.com",
                    PhoneNumber = "111-111-1111",
                    FirstName = "Matthew",
                    LastName = "Baccam",
                    AppID = 100003,
                    PositionTitle = "Guest"
                });

                var houseKeeping = new ApplicationUser()
                {
                    UserName = "jdoe@hotel.com",
                    Email = "jdoe@hotel.com",
                    PhoneNumber = "111-222-3333",
                    FirstName = "Jane",
                    LastName = "Doe",
                    AppID = 100000,
                    PositionTitle = "Housekeeper"
                };

                var frontDesk = new ApplicationUser()
                {
                    UserName = "jsmith@hotel.com",
                    Email = "jsmith@hotel.com",
                    PhoneNumber = "444-555-6666",
                    FirstName = "John",
                    LastName = "Smith",
                    AppID = 100001,
                    PositionTitle = "Front Desk Agent"
                };

                IdentityResult houseKeepingResult = userManager.Create(houseKeeping, adminPassword);
                context.SaveChanges();//updates the database

                if (houseKeepingResult.Succeeded)
                {
                    userManager.AddToRole(houseKeeping.Id, "Housekeeper");
                    context.SaveChanges();
                }

                IdentityResult frontDeskResult = userManager.Create(frontDesk, adminPassword);
                context.SaveChanges();//updates the database

                if (frontDeskResult.Succeeded)
                {
                    userManager.AddToRole(frontDesk.Id, "Front Desk Agent");
                    context.SaveChanges();
                }

                IdentityResult adminResult = userManager.Create(user, adminPassword);
                context.SaveChanges();//updates the database

                if (adminResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();
                }

                foreach (var sampleUser in guestList)
                {
                    IdentityResult result = userManager.Create(sampleUser, adminPassword);
                    context.SaveChanges();//updates the database

                    if (result.Succeeded)
                    {
                        userManager.AddToRole(sampleUser.Id, "Guest");
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
