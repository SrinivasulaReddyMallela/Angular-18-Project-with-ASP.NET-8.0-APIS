using System.Collections.Generic;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IUsersInRoles
    {
        Task<bool> AssignRole(UsersInRoles usersInRoles);
        Task<bool> CheckRoleExists(UsersInRoles usersInRoles);
        Task<bool> RemoveRole(UsersInRoles usersInRoles);
        Task<List<AssignRolesViewModel>> GetAssignRoles();
    }
}