namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    public class JobQueryCriteria : Pageable
    {
        public string name { get; set; }
        public bool enabled { get; set; }
        public List<DateTime> createTime { get; set; }
    }
}
