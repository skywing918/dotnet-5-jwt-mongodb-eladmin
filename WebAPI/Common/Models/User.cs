namespace WebAPI.Common.Models
{
    using AspNetCore.Identity.MongoDbCore.Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;

    public class User : MongoIdentityUser
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string dept_id { get; set; }
        public string nick_name { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }        
        public string avatar_name { get; set; }
        public string avatar_path { get; set; }
        public bool is_admin { get; set; }
        public bool enabled { get; set; }
    }
}
