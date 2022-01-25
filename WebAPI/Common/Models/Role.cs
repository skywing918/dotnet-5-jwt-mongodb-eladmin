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
        public Role()
        {

        }
        public Role(string roleName)
        {
            this.Name = roleName;
        }
    }
}
