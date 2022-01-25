namespace WebAPI.Common.Models
{
    public class Dept : MongoEntity
    {
        public string name { get; set; }
        public bool enabled { get; set; }
        public string pid { get; set; }
    }
}
