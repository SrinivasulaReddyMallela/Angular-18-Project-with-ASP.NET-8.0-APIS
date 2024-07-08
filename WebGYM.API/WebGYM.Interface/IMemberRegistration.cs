using System.Collections.Generic;
using System.Linq;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IMemberRegistration
    {
        Task<int> InsertMember(MemberRegistration memberRegistration);
        Task<long> CheckNameExitsforUpdate(string memberFName, string memberLName, string memberMName);
        Task<bool> CheckNameExits(string memberFName, string memberLName, string memberMName);
        Task<List<MemberRegistrationGridModel>> GetMemberList();
        Task<MemberRegistrationViewModel> GetMemberbyId(int memberId);
        Task<bool> DeleteMember(long memberId);
        Task<int> UpdateMember(MemberRegistration memberRegistration);
        Task<IQueryable<MemberRegistrationGridModel>> GetAll(QueryParameters queryParameters, int userId);
        Task<int> Count(int userId);
        Task<List<MemberResponse>> GetMemberNoList(string memberNo, int userId);
    }
}