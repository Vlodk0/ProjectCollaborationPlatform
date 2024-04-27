

namespace ProjectCollaborationPlatform.Domain.Helpers
{
    public class CustomApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Detail {  get; set; }
    }
}
