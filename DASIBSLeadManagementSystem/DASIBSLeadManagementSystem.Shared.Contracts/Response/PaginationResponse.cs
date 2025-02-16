namespace LeadManagementSystem.Shared.Contracts.Response
{
    public class PaginationResponse
    {
            public int TotalCount { get; set; }
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public bool HasNextPage { get; set; }
            public bool HasPreviousPage { get; set; }
            public List<LeadRecordResponse> Data { get; set; }
  

    }
}
