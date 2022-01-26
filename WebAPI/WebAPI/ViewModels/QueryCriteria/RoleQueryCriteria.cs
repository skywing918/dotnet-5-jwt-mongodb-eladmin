namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    public class RoleQueryCriteria : Pageable
    {
        public string blurry { get; set; }
        public List<DateTime> createTime { get; set; }
    }
}
