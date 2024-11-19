namespace FastCarSales.Services.Data.Tests
{
    using System;
	using FastCarSales.Data;
	using FastCarSales.Data.Models;
	using FastCarSales.Services.Statistics;
	using FastCarSales.Services.Statistics.Models;
	using Microsoft.EntityFrameworkCore;
    using Xunit;
   

    public class StatisticsServiceTests
    {
        [Fact]
        public void TotalWithValidDataShouldReturnCorrectModelAndCounts()
        {
            //Arrange
            var testUser = new ApplicationUser
            {
                FullName = "Test user"
            };

            var testPost = new Post
            {
                PublishedOn = DateTime.UtcNow,
                IsPublic = true,
                IsDeleted = false,
                SellerName = "Test seller",
                SellerPhoneNumber = "Test phone number"
            };

            var testBodyType = new BodyType
            {
                Name = "Saloon"
            };

            var optionsBuilder = new DbContextOptionsBuilder<FastCarSalesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new FastCarSalesDbContext(optionsBuilder.Options);

            dbContext.Posts.Add(testPost);
            dbContext.Users.Add(testUser);
            dbContext.BodyTypes.Add(testBodyType);
            dbContext.SaveChanges();

            var statisticsService = new StatisticsService(dbContext);
            
            //Act
            var result = statisticsService.Total();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<StatisticsServiceModel>(result);
            Assert.Equal(1, result.TotalUsers);
            Assert.Equal(1, result.TotalPosts);
            Assert.Equal(1, result.TotalBodyTypes);
        }

        [Fact]
        public void TotalWithEmptyDatabaseShouldReturnCorrectModelAndCounts()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<FastCarSalesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new FastCarSalesDbContext(optionsBuilder.Options);
            var statisticsService = new StatisticsService(dbContext);
            
            //Act
            var result = statisticsService.Total();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<StatisticsServiceModel>(result);
            Assert.Equal(0, result.TotalUsers);
            Assert.Equal(0, result.TotalPosts);
            Assert.Equal(0, result.TotalBodyTypes);
        }
    }
}
