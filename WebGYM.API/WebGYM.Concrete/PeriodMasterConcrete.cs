using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGYM.Interface;
using WebGYM.Models;

namespace WebGYM.Concrete
{
    public class PeriodMasterConcrete : IPeriodMaster
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public PeriodMasterConcrete(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public async Task<List<PeriodTB>> ListofPeriod()
        {
            var listofPeriod = _databaseContext.PeriodTb.ToList();

            listofPeriod.IndexOf(new PeriodTB()
            {
                Text = "---Select---",
                Value = string.Empty
            }, 0);

            return listofPeriod;
        }
        public async Task InsertRole(PeriodTB periodTB)
        {
            _databaseContext.PeriodTb.Add(periodTB);
            _databaseContext.SaveChanges();
        }
        public async Task<bool> CheckPeriodTBExits(string Text)
        {
            var result = (from PeriodTb in _databaseContext.PeriodTb
                          where PeriodTb.Text == Text
                          select PeriodTb).Count();
            return result > 0 ? true : false;
        }
    }
}
