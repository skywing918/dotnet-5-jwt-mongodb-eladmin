namespace WebAPI.ViewModels
{
    using System;
    public class PagedResponse<T>
    {
        public int totalElements { get; set; }
        public T content { get; set; }
        public PagedResponse()
        {

        }
        public PagedResponse(T data, int totalRecords)
        {
            content = data;
            totalElements = totalRecords;
        }
    }
    
}
