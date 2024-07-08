using System;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IRenewal
    {
        Task<RenewalViewModel> GetMemberNo(string memberNo, int userid);
        Task<bool> CheckRenewalPaymentExists(DateTime newdate, long memberId);
    }
}