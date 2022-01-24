namespace WebAPI.Common.Models
{
    using AspNetCore.Identity.MongoDbCore.Models;
    using System.Collections.Generic;

    public class User : MongoIdentityUser
    {
        public string nick_name { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
    }
}
