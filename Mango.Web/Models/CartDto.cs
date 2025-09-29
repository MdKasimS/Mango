﻿using Mango.Web.Models.Dto;

namespace Mango.Web.Models
{
    public class CartDto
    {
        /// <summary>
        /// Needed when we want to show cart for a specific user
        /// </summary>
        public CartHeaderDto CartHeader { get; set; }

        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
