using System.Collections.Generic;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IPlanMaster
    {
        Task InsertPlan(PlanMaster plan);
        Task<bool> CheckPlanExits(string planName);
        Task<List<PlanMasterDisplayViewModel>> GetPlanMasterList();
        Task<PlanMasterViewModel> GetPlanMasterbyId(int planId);
        Task<bool> DeletePlan(int planId);
        Task<bool> UpdatePlanMaster(PlanMaster planMaster);
        Task<List<ActivePlanModel>> GetActivePlanMasterList(int? schemeId);
        Task<string> GetAmount(int planId, int schemeId);
        Task<int> GetPlanMonthbyPlanId(int? planId);
    }
}