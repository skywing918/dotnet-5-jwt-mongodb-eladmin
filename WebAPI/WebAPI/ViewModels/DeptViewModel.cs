using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    public class ReqDeptViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string enabled { get; set; }
        public string isTop { get; set; }
        public string pid { get; set; }
        public int deptSort { get; set; }
        public int subCount { get; set; }
        public DateTime updateTime { get; set; }
    }

    public class DeptViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public string isTop { get; set; }
        
        public string pid { get; set; }
        public int deptSort { get; set; }
        public int subCount { get; set; }
        public bool hasChildren { get; set; }
        public bool leaf { get; set; }
        public string label { get; set; }
        public DateTime createTime { get; set; }
        public string updateBy { get; set; }
        public DateTime updateTime { get; set; }
    }
    public static class DeptViewModelExtensions
    {
        public static Dept ToModel(this ReqDeptViewModel curr)
        {
            var model = new Dept
            {
                name = curr.name,
                enabled = bool.Parse(curr.enabled),
                update_time = curr.updateTime,
                deptSort = curr.deptSort,
                pid = curr.pid
            };
            return model;
        }

        public static DeptViewModel ToViewModel(this Dept curr)
        {
            var model = new DeptViewModel
            {
                id = curr.Id,
                name = curr.name,
                pid= curr.pid,
                enabled = curr.enabled,
                deptSort = curr.deptSort,
                subCount = curr.sub_count,
                hasChildren = curr.sub_count > 0,
                leaf = curr.sub_count <= 0,
                label = curr.name,
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
