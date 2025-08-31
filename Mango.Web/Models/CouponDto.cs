using Newtonsoft.Json;

namespace Mango.Web.Models
{
    public class CouponDto
    {
        [JsonProperty("id")]
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
