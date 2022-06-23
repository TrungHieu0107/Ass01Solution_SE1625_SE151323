using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        IEnumerable<MemberObject> GetMemberList();
        MemberObject GetMemberByID(int id);
        IEnumerable<MemberObject> FilterByCity(string city);
        IEnumerable<MemberObject> GetMemberByName(string name);
        void InsertMember(MemberObject member);
        void UpdateMember(MemberObject member);
        void DeleteMember(int id);

        string CheckLogin(string email, string password);

        MemberObject GetMemberByEmail(string email);
    }
}
