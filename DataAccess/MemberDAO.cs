using System;
using BusinessObject;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace DataAccess
{
    public class MemberDAO
    {
        private static List<MemberObject> list = new List<MemberObject>() {
            new MemberObject {
                MemberID = 1,
                MemberName = "An",
                DateOfBirth = DateTime.Now,
                City = "TP HCM",
                Country = "Việt Nam",
                Email = "abc@gmail.com",
                Password = "123",
            },
            new MemberObject {
                MemberID = 2,
                MemberName = "Bình",
                DateOfBirth = DateTime.Now,
                City = "Hà Nội",
                Country = "Việt Nam",
                Email = "def@gmail.com",
                Password = "123",
            },
        };

        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();

        private MemberDAO() { }

        public static MemberDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

            public List<MemberObject> GetMemberList => list;

        public MemberObject GetMemberByID(int memberID) { 
            MemberObject member = list.SingleOrDefault(x => x.MemberID == memberID);
            return member;
        }

        public void AddNewMember(MemberObject member) { 
            MemberObject mem = GetMemberByID(member.MemberID);
            if(mem == null) {
                list.Add(member);
            } else {
                throw new Exception("Member is already exists!");
            }
        }

        public void Update(MemberObject member) { 
        MemberObject mem = GetMemberByID((int)member.MemberID);
            if(mem != null) {
                var index = list.IndexOf(mem);
                list[index] = member;
            } else {
                throw new Exception("Member is not found!");
            }
        }

        public void Delete(int memberID) { 
            MemberObject mem = GetMemberByID(((int)memberID));
            if (mem != null) {
                list.Remove(mem); 
            } else {
                throw new Exception("Member is not found!");
            }
        }

        public List<MemberObject> GetMemberByName(string name) {

            var ListSearchResult = new List<MemberObject>();
            foreach (MemberObject member in list) {
                if (member.MemberName.ToLower().Contains(name.ToLower())) { 
                    ListSearchResult.Add(member);
                }
            }

            return ListSearchResult;
        }

        public List<MemberObject> FilterByCity(string city) {
            var ListSearchResult = new List<MemberObject>();
            foreach (MemberObject member in list) {
                if (member.City.ToLower().Contains(city.ToLower())) {
                    ListSearchResult.Add(member);
                }
            }

            return ListSearchResult;
        }

        public string Login(string Email, string Password) {
            string fileName = "appsettings.json";
            string json = File.ReadAllText(fileName);
            Account account = JsonSerializer.Deserialize<Account>(json, null);


            string result = "user";
            if (account != null) {
                if (Email == account.Email && Password == account.Password) {
                    result = account.Role;
                }
                else {
                    MemberObject member = list.SingleOrDefault(x => x.Email == Email);
                    if (member != null) {
                        if (member.Password == Password) {
                            result = "user";
                        } else {
                            throw new Exception("Wrong email or password");
                        }
                    }
                    else {
                        throw new Exception("Wrong email or password");
                    };
                }
                

            } else {
                throw new Exception("Login Error");
            }


            return result;

        }

        public MemberObject GetMemberByEmail(string email) {

            return list.SingleOrDefault(x => x.Email == email);
                }


    }
}

public class Account{
    
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

