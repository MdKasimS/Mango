namespace Mango.Web.Models
{
    public class RequestDto
    {
        public string Url { get; set; }
        public string ApiType { get; set; } = "GET"; 
        public string AccessToken { get; set; }
        public object? Data { get; set; } // Nullable to allow for no data in some requests
    }
}
