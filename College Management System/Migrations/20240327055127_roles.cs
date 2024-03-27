using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace College_Management_System.Migrations
{
    public partial class roles : Migration
    {
        protected override async void Up(MigrationBuilder migrationBuilder)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            string[] roleNames = { "Student", "Staff", "Gamer" };
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
