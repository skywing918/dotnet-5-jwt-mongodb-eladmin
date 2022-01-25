using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    public class DeptViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public string pid { get; set; }
    }
    public static class DeptViewModelExtensions
    {
        public static DeptViewModel ToViewModel(this Dept dept)
        {
            var model = new DeptViewModel
            {
                id = dept.Id,
                name = dept.name,
                pid=dept.pid
            };
            return model;
        }

        public static List<DeptViewModel> ToViewModel(this List<Dept> depts)
        {
            var models = depts.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}
