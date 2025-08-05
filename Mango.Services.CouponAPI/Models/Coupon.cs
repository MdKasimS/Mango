namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string? CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public double MinAmount { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public DateTime ExpiryDate { get; set; }
        //public bool IsActive { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? UpdatedBy { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public string? Description { get; set; }
        //public string? CouponType { get; set; } // e.g., "Percentage", "Amount"
        //public string? ProductName { get; set; } // Optional, if the coupon is specific to a product
        //public string? ProductCategory { get; set; } // Optional, if the coupon is specific to a category
        //public string? ProductDescription { get; set; }



    }
}
