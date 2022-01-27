using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;

namespace WebAPI.ViewModels
{
    public class JobViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public int jobSort { get; set; }
        public DateTime createTime { get; set; }
    }

    public static class JobViewModelExtensions
    {
        public static Job ToModel(this JobViewModel curr)
        {
            var model = new Job
            {
               name = curr.name,
               enabled = curr.enabled,
               job_sort = curr.jobSort,
               create_time = curr.createTime
            };
            return model;
        }

        public static JobViewModel ToViewModel(this Job curr)
        {
            var model = new JobViewModel
            {
                id = curr.Id,
                name = curr.name,                
                enabled = curr.enabled,
                jobSort = curr.job_sort,
                createTime = curr.create_time
            };
            return model;
        }

        public static List<JobViewModel> ToViewModel(this List<Job> jobs)
        {
            var models = jobs.Select(u => u.ToViewModel()).ToList();
            return models;
        }
    }
}
