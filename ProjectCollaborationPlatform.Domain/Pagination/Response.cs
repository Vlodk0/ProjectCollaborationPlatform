namespace ProjectCollaborationPlatform.Domain.Pagination
{
    public class Response<T>
    {
        public T Data { get; set; } 
        public bool Succeded { get; set; }
        public string[] Errors { get; set; }
        public string Message {  get; set; }
        public Response()
        {
            
        }
        public Response(T data)
        {
            Succeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
    }
}
