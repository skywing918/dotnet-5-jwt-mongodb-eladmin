namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    public class DeptQueryCriteria : Pageable
    {
        public string name { get; set; }
        public string? pid { get; set; }
        public List<DateTime> createTime { get; set; }
    }
}
