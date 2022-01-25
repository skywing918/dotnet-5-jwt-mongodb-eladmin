using System.Collections.Generic;
using System.Linq;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    public class UserViewModel
    {
        public int id { get; set; }

        public DeptViewModel dept { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string nickName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public string avatarName { get; set; }
        public string avatarPath { get; set; }
        public bool enabled { get; set; }
    }
    public static class UserViewModelExtensions
    {
        public static UserViewModel ToViewModel(this User user)
        {
            var model = new UserViewModel
            {
                dept = new DeptViewModel
                {
                    name = "华南分部",
                },                
                username = user.UserName,
                nickName = user.nick_name,
                email = user.Email
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
