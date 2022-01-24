namespace WebAPI.Common.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string create_by { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime update_time { get; set; }
    }
}
