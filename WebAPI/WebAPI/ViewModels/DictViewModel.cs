namespace WebAPI.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using WebAPI.Common.Models;

    public class DictViewModel
    {
        public string id { get; set; }
        public List<DictDetailViewModel> dictDetails { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public static class DictViewModelExtensions
    {
        public static Dict ToModel(this DictViewModel curr)
        {
            var model = new Dict
            {
                name = curr.name,
                description = curr.description
            };
            return model;
        }

        public static DictViewModel ToViewModel(this Dict curr)
        {
            var model = new DictViewModel
            {
                id = curr.Id,
                name = curr.name,
                description = curr.description,
                dictDetails = curr.dictDetails?.ToViewModel()
            };
            return model;
        }

        public static List<DictViewModel> ToViewModel(this List<Dict> dicts)
        {
            var models = dicts.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}
