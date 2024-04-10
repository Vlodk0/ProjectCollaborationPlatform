namespace ProjectCollaborationPlatform.Domain.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber {  get; set; }
        public int PageSize { get; set; }
        public int Payment {  get; set; }
        public string SortDirection { get; set; }
        public string SortColumn {  get; set; }

        public PaginationFilter()
        {
            PageNumber = 0;
            PageSize = 10;
            Payment = 0;
            SortDirection = "asc";
            SortColumn = string.Empty;
        }

        public PaginationFilter(int pageNumber, int pageSize, string sortColumn, string sortDirection)
        {
            PageNumber = pageNumber < 1? 1 : pageNumber;    
            PageSize = pageSize > 10 ? 10 : pageSize;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 0 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
