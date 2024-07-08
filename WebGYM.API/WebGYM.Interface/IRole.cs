using System.Collections.Generic;
using WebGYM.Models;

namespace WebGYM.Interface
{
    public interface IRole
    {
        Task InsertRole(Role role);
        Task<bool> CheckRoleExits(string roleName);
        Task<Role> GetRolebyId(int roleId);
        Task<bool> DeleteRole(int roleId);
        Task<bool> UpdateRole(Role role);
        Task<List<Role>> GetAllRole();
    }
}