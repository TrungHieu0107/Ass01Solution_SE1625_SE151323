using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        string IMemberRepository.CheckLogin(string email, string password) =>MemberDAO.Instance.Login(email, password); 

        void IMemberRepository.DeleteMember(int id) => MemberDAO.Instance.Delete(id);

        IEnumerable<MemberObject> IMemberRepository.FilterByCity(string city) => MemberDAO.Instance.FilterByCity(city);

        MemberObject IMemberRepository.GetMemberByEmail(string email) => MemberDAO.Instance.GetMemberByEmail(email);

        MemberObject IMemberRepository.GetMemberByID(int id) => MemberDAO.Instance.GetMemberByID(id);

        IEnumerable<MemberObject> IMemberRepository.GetMemberByName(string name) => MemberDAO.Instance.GetMemberByName(name);

        IEnumerable<MemberObject> IMemberRepository.GetMemberList() => MemberDAO.Instance.GetMemberList;

        void IMemberRepository.InsertMember(MemberObject member) => MemberDAO.Instance.AddNewMember(member);

        void IMemberRepository.UpdateMember(MemberObject member) => MemberDAO.Instance.Update(member);
    }
}
