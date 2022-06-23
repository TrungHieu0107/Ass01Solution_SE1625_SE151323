using System;

namespace BusinessObject
{
    public class MemberObject
    {
       public int MemberID { get; set; }
        public string MemberName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public MemberObject()
        {
        }

        public MemberObject(int memberID, string memberName, DateTime dateOfBirth, string password, string email, string city, string country)
        {
            MemberID = memberID;
            MemberName = memberName;
            DateOfBirth = dateOfBirth;
            Password = password;
            Email = email;
            City = city;
            Country = country;
        }

    }
}
