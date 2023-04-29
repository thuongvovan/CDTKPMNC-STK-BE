namespace CDTKPMNC_STK_BE.Models
{
    public class ResponseMessage
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
