namespace HRSystem.Application.Dtos
{
    public class ErrorDto
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
