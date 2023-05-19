namespace CDTKPMNC_STK_BE.Controllers
{
    // public record ResponseMessage(bool Success, string? Message = null, object? Data = null);

    public class ResponseMessage
    {
        public ResponseMessage() { }
        public ResponseMessage(bool success, string? message = null, object? data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
