using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Services.Posts.Models
{
    public interface ISortableModel
    {
        PostsSorting Sorting { get; set; }
    }
}
