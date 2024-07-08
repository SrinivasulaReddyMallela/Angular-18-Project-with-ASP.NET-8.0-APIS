using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGYM.Models;

namespace WebGYM.Interface
{
  public  interface IPeriodMaster
    {
        Task<List<PeriodTB>> ListofPeriod();
        Task InsertRole(PeriodTB periodTB);
        Task<bool> CheckPeriodTBExits(string Text);
    }
}
