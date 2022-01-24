namespace WebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebAPI.Common.Models;
    using WebAPI.Common.Models.Enum;

    public class MenuViewModel
    {
        public bool alwaysShow { get; set; }
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
            this IEnumerable<Menu> collection,
            Func<Menu, string> id_selector,
            Func<Menu, string> parent_id_selector,
            string root_id = default(string))
        {

            foreach (var c in collection.Where(c => EqualityComparer<string>.Default.Equals(parent_id_selector(c), root_id)))
            {
                var currComponent = "Layout";

                if (c.type== MenuType.Menu)
                {
                    currComponent = c.component;
                }

                var childs = collection.ToViewModel(id_selector, parent_id_selector, id_selector(c));
                yield return new MenuViewModel
                {
                    alwaysShow = !c.hidden,
                    component = currComponent,
                    hidden = c.hidden,
                    meta = new MetaModel
                    {
                        icon = c.icon,
                        noCache = c.i_frame,
                        title = c.title
                    },
                    name = c.title,
                    path = $"/{c.path}",

                    Children = childs.Count() > 0 ? childs : null
                };
            }
        }
    }
}
