namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    public class UserQueryCriteria:Pageable
    {
        public string blurry { get; set; }
        public bool enabled { get; set; }
        public List<DateTime> createTime { get; set; }
    } 
}
