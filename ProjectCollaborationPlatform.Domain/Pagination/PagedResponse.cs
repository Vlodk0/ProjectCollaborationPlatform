namespace ProjectCollaborationPlatform.Domain.Pagination
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; } 
        public int PageSize {  get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPage {  get; set; }
        public int TotalRecords {  get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage {  get; set; }  

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeded = true;
            this.Errors = null;
        }
    }
}
