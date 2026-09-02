using Mango.Web.Models.Dto;

namespace Mango.Web.Models
{
    /* Properties are kept as it is
     * to keep sync with course. But StripeRequestDto is 
     * PaymentRequestDto in my codebase. Because I can use any other
     * pay methods.
     */

    public class PaymentRequestDto
    {
        public string StripeSessionUrl { get; set; }
        public string StripeSessionId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
    }
}
