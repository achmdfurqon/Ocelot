using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersAPI.Models
{
    public class User
    {
        public string NIK { get; set; }
        public string Name { get; set; }
        public User(string nik, string name)
        {
            NIK = nik;
            Name = name;
        }
    }

    public class Audience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
