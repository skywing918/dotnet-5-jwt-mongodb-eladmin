namespace WebAPI.Common.Models
{
    using AspNetCore.Identity.MongoDbCore.Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;

    public class Role : MongoIdentityRole<Guid>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> MenuIds { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> deptIds { get; set; }
        public string dataScope { get; set; }
        public int level { get; set; }
        public string description { get; set; }
        public string create_by { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime update_time { get; set; }
        public Role()
        {

        }
        public Role(string roleName)
        {
            this.Name = roleName;
        }
    }
}
