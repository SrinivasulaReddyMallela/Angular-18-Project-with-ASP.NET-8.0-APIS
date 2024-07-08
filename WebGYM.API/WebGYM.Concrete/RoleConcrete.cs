using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebGYM.Interface;
using WebGYM.Models;

namespace WebGYM.Concrete
{
    public class RoleConcrete : IRole
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public RoleConcrete(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> CheckRoleExits(string roleName)
        {
            var result = (from role in _context.Role
                          where role.RoleName == roleName
                          select role).Count();

            return result > 0 ? true : false;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var roledata = (from role in _context.Role
                            where role.RoleId == roleId
                            select role).FirstOrDefault();

            if (roledata != null)
            {
                _context.Role.Remove(roledata);
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

        public async Task<Role> GetRolebyId(int roleId)
        {
            var result = (from role in _context.Role
                          where role.RoleId == roleId
                          select role).FirstOrDefault();

            return result;
        }

        public async Task<List<Role>> GetAllRole()
        {
            var result = (from role in _context.Role
                select role).ToList();

            return result;
        }

        public async Task InsertRole(Role role)
        {
            _context.Role.Add(role);
            _context.SaveChanges();
        }

        public async Task<bool> UpdateRole(Role role)
        {
            _context.Entry(role).Property(x => x.Status).IsModified = true;
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
    }
}
