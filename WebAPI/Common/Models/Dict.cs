namespace WebAPI.Common.Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;

    public class Dict : MongoEntity
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<DictDetail> dictDetails { get; set; }
    }
}
