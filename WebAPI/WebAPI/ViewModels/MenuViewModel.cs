namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebAPI.Common.Models;
    using WebAPI.Common.Models.Enum;

    public class MenuVoViewModel
    {
        public bool? alwaysShow { get; set; }

        public IEnumerable<MenuVoViewModel> Children { get; set; }

        public string component { get; set; }
        public bool hidden { get; set; }
        public MetaModel meta { get; set; }
        public string name { get; set; }
        public string path { get; set; }

        public string redirect { get; set; }

    }

    public class MetaModel
    {
        public string icon { get; set; }
        public bool noCache { get; set; }
        public string title { get; set; }
    }

    public class MenuViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string componentName { get; set; }
        public int menuSort { get; set; }
        public string component { get; set; }
        public string path { get; set; }
        public int type { get; set; }
        public string permission { get; set; }
        public string icon { get; set; }
        public bool cache { get; set; }
        public bool hidden { get; set; }
        public string pid { get; set; }
        public bool iFrame { get; set; }       
        public DateTime createTime { get; set; }
        public bool hasChildren { get; set; }
        public bool leaf { get; set; }
        public string label { get; set; }
    }

    public static class MenuViewModelExtensions
    {
        public static IEnumerable<MenuVoViewModel> ToVoViewModel(
            this IEnumerable<Menu> menuDtos,
            Func<Menu, string> id_selector,
            Func<Menu, string> parent_id_selector,
            string root_id = default(string))
        {

            foreach (var menuDTO in menuDtos.Where(c => EqualityComparer<string>.Default.Equals(parent_id_selector(c), root_id)))
            {
                var menuDtoList = menuDtos.ToVoViewModel(id_selector, parent_id_selector, id_selector(menuDTO));
                var menuVo = new MenuVoViewModel();
                menuVo.name = string.IsNullOrEmpty(menuDTO.component) ? menuDTO.title : menuDTO.component;
                menuVo.path = menuDTO.pid == null ? $"/{menuDTO.path}" : menuDTO.path;
                menuVo.hidden = menuDTO.hidden;

                if (!menuDTO.i_frame)
                {
                    if(menuDTO.pid==null)
                    {
                        menuVo.component = string.IsNullOrEmpty(menuDTO.component) ? "Layout" : menuDTO.component;
                    } 
                    else if(menuDTO.type == MenuType.Folder)
                    {
                        menuVo.component = string.IsNullOrEmpty(menuDTO.component) ? "ParentView" : menuDTO.component;
                    }
                    else
                    {
                        menuVo.component = menuDTO.component;
                    }
                }
                menuVo.meta = new MetaModel
                {
                    icon = menuDTO.icon,
                    noCache = !menuDTO.cache,
                    title = menuDTO.title
                };
                if (menuDtoList.Count() > 0)
                {
                    menuVo.alwaysShow = true;
                    menuVo.redirect = "noredirect";
                    menuVo.Children = menuDtoList;
                }
                else if (menuDTO.pid == null)
                {
                    MenuVoViewModel menuVo1 = new MenuVoViewModel();
                    menuVo1.meta = menuVo.meta;
                    if (!menuDTO.i_frame)
                    {
                        menuVo1.path = "index";
                        menuVo1.name = menuVo.name;
                        menuVo1.component = menuVo.component;
                    }
                    else
                    {
                        menuVo1.path = menuDTO.path;
                    }
                    menuVo.name = null;
                    menuVo.meta = null;
                    menuVo.component = "Layout";
                    menuVo.Children = new List<MenuVoViewModel> { menuVo1 };
                }
                yield return menuVo;
            }
        }

        public static MenuViewModel ToViewModel(this Menu curr)
        {
            var model = new MenuViewModel
            {
                cache = curr.cache,
                createTime = curr.create_time,
                component = curr.component,
                componentName = curr.name,
                hidden = curr.hidden,
                permission = curr.permission,
                iFrame = curr.i_frame,
                icon = curr.icon,
                id = curr.Id,                
                menuSort = curr.menu_sort,
                path = curr.path,
                title = curr.title,
                type = (int)curr.type,
                hasChildren = curr.sub_count > 0,
                leaf = curr.sub_count <= 0,
                label = curr.title,                
            };
            return model;
        }

        public static List<MenuViewModel> ToViewModel(this IEnumerable<Menu> menus)
        {
            var models = menus.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}
