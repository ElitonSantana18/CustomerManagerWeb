namespace CustomerManagerWeb.Models
{
    public class MessageResponse<T> where T:class
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
