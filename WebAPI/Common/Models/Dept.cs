namespace WebAPI.Common.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;
    public class Dept : MongoEntity
    {
        public List<Guid> roleIds { get; set; }
        public int deptSort { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string pid { get; set; }
        public int sub_count { get; set; }
        
    }
}
