using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    

    public class UserViewModel
    {
        public Guid? id { get; set; }
        public List<RoleViewModel> roles { get; set; }
        public List<JobViewModel> jobs { get; set; }
        public DeptViewModel dept { get; set; }
        public string username { get; set; }
        public string nickName { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
        public string gender { get; set; }
        public string avatarName { get; set; }
        public string avatarPath { get; set; }
        public object enabled { get; set; }
    }
    public static class UserViewModelExtensions
    {      

        public static User ToModel(this UserViewModel curr)
        {
            var model = new User
            {
                UserName = curr.username,
                gender = curr.gender,
                enabled =bool.Parse(curr.enabled.ToString()),
                Email = curr.email,
                nick_name = curr.nickName,
                phone = curr.phone,
                Roles = curr.roles?.Select(x => x.id.Value).ToList(),
                job_ids = curr.jobs?.Select(x => x.id).ToList(),
                dept_id = curr.dept.id
            };
            return model;
        }

        public static UserViewModel ToViewModel(this User user)
        {
            var model = new UserViewModel
            {
                id = user.Id,
                dept = new DeptViewModel
                {
                    id = user.dept_id,
                    name = "华南分部",
                },
                username = user.UserName,
                nickName = user.nick_name,
                email = user.Email,
                enabled = user.enabled,
                gender = user.gender,
                phone = user.phone,
                roles = user.Roles?.Select(x=>new RoleViewModel { id = x}).ToList(),
                jobs = user.job_ids?.Select(x => new JobViewModel { id = x }).ToList(),
            };
            return model;
        }

        public static List<UserViewModel> ToViewModel(this List<User> users)
        {
            var models = users.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}
