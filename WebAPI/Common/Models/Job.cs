namespace WebAPI.Common.Models
{
    public class Job : MongoEntity
    {
        public int job_sort { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
    }
}
