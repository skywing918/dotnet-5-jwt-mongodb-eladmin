namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebAPI.Common.Models;
    using WebAPI.Common.Models.Enum;

    public class MenuViewModel
    {
        public bool? alwaysShow { get; set; }

        public IEnumerable<MenuViewModel> Children { get; set; }

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

    public static class MenuViewModelExtensions
    {
        public static IEnumerable<MenuViewModel> ToViewModel(
            this IEnumerable<Menu> menuDtos,
            Func<Menu, string> id_selector,
            Func<Menu, string> parent_id_selector,
            string root_id = default(string))
        {

            foreach (var menuDTO in menuDtos.Where(c => EqualityComparer<string>.Default.Equals(parent_id_selector(c), root_id)))
            {
                var menuDtoList = menuDtos.ToViewModel(id_selector, parent_id_selector, id_selector(menuDTO));
                var menuVo = new MenuViewModel();
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
                    MenuViewModel menuVo1 = new MenuViewModel();
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
                    menuVo.Children = new List<MenuViewModel> { menuVo1 };
                }
                yield return menuVo;
            }
        }
    }
}
