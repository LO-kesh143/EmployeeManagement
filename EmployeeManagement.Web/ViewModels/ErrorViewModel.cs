namespace EmployeeManagement.Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public bool HasRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
