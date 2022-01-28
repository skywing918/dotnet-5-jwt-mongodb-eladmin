using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    public class RoleViewModel
    {
        public Guid? id { get; set; }
        public IEnumerable<MenuViewModel> menus { get; set; }
        public IEnumerable<DeptViewModel> depts { get; set; }
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
        public static Role ToModel(this RoleViewModel curr)
        {
            var model = new Role
            {                
                Name = curr.name,
                dataScope = curr.dataScope,
                update_time = curr.updateTime,
                level = curr.level,
                description = curr.description
            };
            return model;
        }

        public static RoleViewModel ToViewModel(this Role curr)
        {
            // build menu
            var menuObjs = curr.MenuIds?.Select(id => new MenuViewModel { id = id });

            var model = new RoleViewModel
            {
                id = curr.Id,
                name = curr.Name,
                dataScope = curr.dataScope,
                level = curr.level,
                description = curr.description,
                createTime = curr.create_time,
                updateBy = curr.update_by,
                updateTime = curr.update_time,
                menus = menuObjs
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