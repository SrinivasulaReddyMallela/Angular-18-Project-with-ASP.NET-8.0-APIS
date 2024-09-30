namespace IdentityStandaloneMfa.Common
{
    public class DbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        //private readonly IRole _role;
        //private readonly IUsers _users;
        //private readonly IUsersInRoles _usersInRoles;
        //private readonly IPeriodMaster _periodMaster;
        public DbInitializer(ILogger<DbInitializer> logger/*, IRole role, IUsers users, IUsersInRoles usersInRoles, IPeriodMaster periodMaster*/)
        {
            _logger = logger;
            //_role = role;
            //_users = users;
            //_usersInRoles = usersInRoles;
            //_periodMaster = periodMaster;
        }

        public DbInitializer()
        {

        }
        public async Task CreateDefaultUsers()
        {
            try
            {
                //if (!await _users.CheckUsersExits("normalUser@gmail.com"))
                //{
                //    Users users = new Users();
                //    users.UserName = "normalUser@gmail.com";
                //    users.EmailId = "normalUser@gmail.com";
                //    users.FullName = "Normal User";
                //    users.Contactno = "9999999999";
                //    users.CreatedDate = DateTime.Now;
                //    users.Status = true;
                //    users.Createdby = 1;
                //    users.RefreshToken = " ";
                //    users.Password = EncryptionLibrary.EncryptText("123456");
                //    var normaluserstatus = await _users.InsertUsers(users);
                //    if (normaluserstatus)
                //    {
                //        var usersInRoles = new UsersInRoles();
                //        var UserData = (await _users.GetAllUsers()).ToList().Where(x => x.UserName == "normalUser@gmail.com").FirstOrDefault();
                //        var RoleData = (await _role.GetAllRole()).ToList().Where(x => x.RoleName == "user").FirstOrDefault();
                //        usersInRoles.UserId = UserData.UserId;
                //        usersInRoles.RoleId = RoleData.RoleId;
                //        if (!await _usersInRoles.CheckRoleExists(usersInRoles))
                //        {
                //            usersInRoles.UserRolesId = 0;
                //            await _usersInRoles.AssignRole(usersInRoles);
                //        }
                //    }
                //}

                //if (!await _users.CheckUsersExits("adminUser@gmail.com"))
                //{
                //    Users users = new Users();
                //    users.UserName = "adminUser@gmail.com";
                //    users.EmailId = "adminUser@gmail.com";
                //    users.FullName = "Admin User";
                //    users.Contactno = "9999999999";
                //    users.CreatedDate = DateTime.Now;
                //    users.Status = true;
                //    users.Createdby = 1;
                //    users.RefreshToken = " ";
                //    users.Password = EncryptionLibrary.EncryptText("123456");
                //    var normaluserstatus = await _users.InsertUsers(users);
                //    if (normaluserstatus)
                //    {
                //        var usersInRoles = new UsersInRoles();
                //        var UserData = (await _users.GetAllUsers()).ToList().Where(x => x.UserName == "adminUser@gmail.com").FirstOrDefault();
                //        var RoleData = (await _role.GetAllRole()).ToList().Where(x => x.RoleName == "admin").FirstOrDefault();
                //        usersInRoles.UserId = UserData.UserId;
                //        usersInRoles.RoleId = RoleData.RoleId;
                //        if (!await _usersInRoles.CheckRoleExists(usersInRoles))
                //        {
                //            usersInRoles.UserRolesId = 0;
                //            await _usersInRoles.AssignRole(usersInRoles);
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
            }
        }

        public async Task CreateRoles()
        {
            try
            {
                //if (!await _role.CheckRoleExits("admin"))
                //    await _role.InsertRole(new WebGYM.Models.Role { RoleName = "admin", Status = true });
                //if (!await _role.CheckRoleExits("user"))
                //    await _role.InsertRole(new WebGYM.Models.Role { RoleName = "user", Status = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
            }
        }
        //public async Task CreatePeriodTB()
        //{
        //    try
        //    {
        //        if (!await _periodMaster.CheckPeriodTBExits("One Month"))
        //            await _periodMaster.InsertRole(new PeriodTB { PeriodID = 0, Text = "One Month", Value = "One Month" });
        //        if (!await _periodMaster.CheckPeriodTBExits("Two Months"))
        //            await _periodMaster.InsertRole(new PeriodTB { PeriodID = 0, Text = "Two Months", Value = "Two Months" });
        //        if (!await _periodMaster.CheckPeriodTBExits("Three Months"))
        //            await _periodMaster.InsertRole(new PeriodTB { PeriodID = 0, Text = "Three Months", Value = "Three Months" });
        //        if (!await _periodMaster.CheckPeriodTBExits("Four Months"))
        //            await _periodMaster.InsertRole(new PeriodTB { PeriodID = 0, Text = "Four Months", Value = "Four Months" });
        //        if (!await _periodMaster.CheckPeriodTBExits("Six Months"))
        //            await _periodMaster.InsertRole(new PeriodTB { PeriodID = 0, Text = "Six Months", Value = "Six Months" });
        //        if (!await _periodMaster.CheckPeriodTBExits("One Year"))
        //            await _periodMaster.InsertRole(new PeriodTB { PeriodID = 0, Text = "One Year", Value = "One Year" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
        //    }
        //}
    }
}
