namespace WebAPI.Common.Models
{
    using AspNetCore.Identity.MongoDbCore.Models;
    using System;
    public class Role : MongoIdentityRole<Guid>
    {
        public Role()
        {

        }
        public Role(string roleName)
        {
            this.Name = roleName;
        }
    }
}
