namespace WebAPI.Common.Models
{
    public class DictDetail : MongoEntity
    {
        public string dict_id { get; set; }
        public DictSmall dict { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public int dict_sort { get; set; }
    }
}
