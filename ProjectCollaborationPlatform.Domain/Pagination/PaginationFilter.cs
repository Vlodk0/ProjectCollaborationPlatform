using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            PageNumber = 1;
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
    }
}
