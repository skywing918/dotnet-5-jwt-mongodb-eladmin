﻿namespace WebAPI.Common.Services
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebAPI.Common.Models;
    public class RoleService
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleService(RoleManager<Role> roleManager)
        {            
            _roleManager = roleManager;
        }
        public async Task<IEnumerable<Role>> findById(IEnumerable<string> Ids)
        {
            var results = new List<Role>();
            foreach(var curr in Ids)
            {
                var role = await _roleManager.FindByIdAsync(curr);
                results.Add(role);
            }
            
            return results;
        }
    }
}
