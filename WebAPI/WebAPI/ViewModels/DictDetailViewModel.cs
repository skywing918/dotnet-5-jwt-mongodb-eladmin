namespace WebAPI.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using WebAPI.Common.Models;

    public class DictDetailViewModel
    {
        public string id { get; set; }
        public DictViewModel dict { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public int dictSort { get; set; }
    }

    public static class DictDetailViewModelExtensions
    {
        public static DictDetail ToModel(this DictDetailViewModel curr)
        {
            var model = new DictDetail            
            {
                Id = curr.id,
                label = curr.label,
                value = curr.value,
                dict_sort = curr.dictSort
            };
            return model;
        }

        public static DictDetailViewModel ToViewModel(this DictDetail curr)
        {
            var model = new DictDetailViewModel
            {
                id = curr.Id,
                label = curr.label,
                value = curr.value,
                dictSort = curr.dict_sort
            };
            return model;
        }

        public static List<DictDetailViewModel> ToViewModel(this List<DictDetail> dicts)
        {
            var models = dicts.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}
