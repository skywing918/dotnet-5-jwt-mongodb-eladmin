namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    public class MenuQueryCriteria : Pageable
    {
        public string blurry { get; set; }
        public List<DateTime> createTime { get; set; }
        public string? pid { get; set; }
}
}
