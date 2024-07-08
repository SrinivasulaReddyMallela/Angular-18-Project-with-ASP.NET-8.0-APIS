using System.Collections.Generic;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IUsers
    {
        Task<bool> InsertUsers(Users user);
        Task<bool> CheckUsersExits(string username);
        Task<Users> GetUsersbyId(int userid);
        Task<bool> DeleteUsers(int userid);
        Task<bool> UpdateUsers(Users role);
        Task<List<Users>> GetAllUsers();
        Task<bool> AuthenticateUsers(string username, string password);
        Task<LoginResponse> GetUserDetailsbyCredentials(string username);
        Task<bool> SaveTokenbyUser(Users user);
    }
}