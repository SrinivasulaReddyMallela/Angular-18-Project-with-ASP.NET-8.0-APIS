using System.Collections.Generic;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IReports
    {
        Task<List<MemberDetailsReportViewModel>> Generate_AllMemberDetailsReport();
        Task<List<YearwiseReportViewModel>> Get_YearwisePayment_details(string year);
        Task<List<MonthWiseReportViewModel>> Get_MonthwisePayment_details(string monthId);
        Task<List<RenewalReportViewModel>> Get_RenewalReport(RenewalReportRequestModel renewalReport);
    }
}