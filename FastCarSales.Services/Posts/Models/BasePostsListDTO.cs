﻿namespace FastCarSales.Services.Posts.Models
{
    using System.Collections.Generic;

    public class BasePostsListDTO
    {
        public IEnumerable<BasePostInListDTO> Posts { get; init; }
    }
}