namespace FastCarSales.Services.Statistics
{
    using System.Linq;
    using Data;
    using Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly FastCarSalesDbContext data;

        public StatisticsService(FastCarSalesDbContext data)
        {
            this.data = data;
        }

        public StatisticsServiceModel Total()
        {
            var totalUsers = this.data.Users.Count();
            var totalPosts = this.data.Posts.Count(p => !p.IsDeleted && p.IsPublic);
            var totalBodyTypes = this.data.BodyTypes.Count();

            return new StatisticsServiceModel
            {
                TotalUsers = totalUsers,
                TotalPosts = totalPosts,
                TotalBodyTypes = totalBodyTypes,
            };
        }
    }
}
