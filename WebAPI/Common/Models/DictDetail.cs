
namespace WebAPI.Common.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    public class DictDetail 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DictSmall dict { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public int dict_sort { get; set; }
    }
}
