using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    public class RoleViewModel
    {
        public Guid id { get; set; }
        public List<MenuViewModel> menus { get; set; }
        public List<DeptViewModel> depts { get; set; }
        public string name { get; set; }
        public string dataScope { get; set; }        
        public int level { get; set; }
        public string description { get; set; }
        public DateTime createTime { get; set; }
        public string updateBy { get; set; }
        public DateTime updateTime { get; set; }
    }

    public static class RoleViewModelExtensions
    {
        public static RoleViewModel ToViewModel(this Role role)
        {
            var model = new RoleViewModel
            {
                id = role.Id,
                name = role.Name,
                dataScope = role.dataScope,
                level = role.level,
                description = role.description,
                createTime = role.create_time,
                updateBy = role.update_by,
                updateTime = role.update_time
            };
            return model;
        }

        public static List<RoleViewModel> ToViewModel(this IEnumerable<Role> roles)
        {
            var models = roles.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}