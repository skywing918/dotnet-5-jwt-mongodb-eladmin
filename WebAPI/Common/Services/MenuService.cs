namespace WebAPI.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebAPI.Common.Helper;
    using WebAPI.Common.Models;
    using WebAPI.Common.Models.Enum;

    public class MenuService
    {
        private static readonly string collectionName = "Menus";

        public async Task<IEnumerable<Menu>> FindByUser(string userId)
        {
            return new List<Menu>
            {
                new Menu
                {
                    Id="1",
                    pid=null,
                    sub_count=7,
                    type = MenuType.Folder,
                    title = "系统管理",
                    name =null,
                    component=null,
                    menu_sort=1,
                    icon="system",
                    path="system",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission=null,
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                new Menu
                {
                    Id="2",
                    pid="1",
                    sub_count=3,
                    type = MenuType.Menu,
                    title = "用户管理",
                    name ="User",
                    component="system/user/index",
                    menu_sort=2,
                    icon="peoples",
                    path="user",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="user:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
            };
        }

        public async Task<Menu> Create(Menu menu)
        {
            var result = await MongoDbHelper.AddRecord(collectionName, menu).ConfigureAwait(false);
            return result;
        }
    }
}
