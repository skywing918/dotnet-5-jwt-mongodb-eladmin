
namespace WebAPI.Common.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using WebAPI.Common.Models.Enum;

    public class Menu : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string pid { get; set; }
        public int sub_count { get; set; }
        public MenuType type { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string component { get; set; }
        public int menu_sort { get; set; }
        public string icon { get; set; }
        public string path { get; set; }
        public bool i_frame { get; set; }
        public bool cache { get; set; }
        public bool hidden { get; set; }
        public string permission { get; set; }
    }
}
