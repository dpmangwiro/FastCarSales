using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Services.Posts.Models
{
    public enum PostsSorting
    {
        NewestFirst = 0,
        OldestFirst = 1,
        PriceHighestFirst = 2,
        PriceLowestFirst = 3,
        HorsepowerHighestFirst = 4,
        HorsepowerLowestFirst = 5,
        CarYearNewestFirst = 6,
        CarYearOldestFirst = 7
    }
}
