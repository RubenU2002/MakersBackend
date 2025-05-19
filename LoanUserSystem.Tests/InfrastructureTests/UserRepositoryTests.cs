using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanUserSystem.Tests.InfrastructureTests
{
    public class UserRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LoanUserTestDb")
                .Options;
            var context = new ApplicationDbContext(options);
            context.Users.Add(new User("Test User", "test@example.com", "User", "SecurePassword"));
            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            var context = await GetDatabaseContext();
            var userRepository = new UserRepository(context);
            var user = await userRepository.GetByIdAsync(1);

            Assert.NotNull(user);
            Assert.Equal("Test User", user.Name);
        }
    }
}
