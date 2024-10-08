﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGYM.Interface;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.Concrete
{
    public class UsersConcrete : IUsers
    {

        private readonly DatabaseContext _context;
        public UsersConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUsersExits(string username)
        {
            var result = (from user in _context.Users
                          where user.UserName == username
                          select user).Count();

            return result > 0 ? true : false;
        }

        public async Task<bool> AuthenticateUsers(string username, string password)
        {
            var result = (from user in _context.Users
                          where user.UserName == username && user.Password == password
                          select user).Count();



            return result > 0 ? true : false;


        }

        public async Task<LoginResponse> GetUserDetailsbyCredentials(string username)
        {
            try
            {
                var result = (from user in _context.Users
                    join userinrole in _context.UsersInRoles on user.UserId equals userinrole.UserId
                              where user.UserName == username 
                              select new LoginResponse
                              {
                                  UserId = user.UserId,
                                  RoleId = userinrole.RoleId,
                                  Status = user.Status,
                                  UserName = user.UserName
                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> DeleteUsers(int userId)
        {
            var removeuser = (from user in _context.Users
                              where user.UserId == userId
                              select user).FirstOrDefault();
            if (removeuser != null)
            {
                _context.Users.Remove(removeuser);
                var result = _context.SaveChanges();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Users>> GetAllUsers()
        {
            var result = (from user in _context.Users
                          where user.Status == true
                          select user).ToList();

            return result;
        }

        public async Task<Users> GetUsersbyId(int userId)
        {
            var result = (from user in _context.Users
                          where user.UserId == userId
                          select user).FirstOrDefault();

            return result;
        }

        public async Task<bool> InsertUsers(Users user)
        {
            _context.Users.Add(user);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateUsers(Users user)
        {
            _context.Entry(user).Property(x => x.EmailId).IsModified = true;
            _context.Entry(user).Property(x => x.Contactno).IsModified = true;
            _context.Entry(user).Property(x => x.Status).IsModified = true;
            _context.Entry(user).Property(x => x.FullName).IsModified = true;
            _context.Entry(user).Property(x => x.Password).IsModified = true;
            _context.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _context.Entry(user).Property(x => x.RefreshTokenExpiryTime).IsModified = true;
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> SaveTokenbyUser(Users user)
        {
            _context.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _context.Entry(user).Property(x => x.RefreshTokenExpiryTime).IsModified = true;
            var result =await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
