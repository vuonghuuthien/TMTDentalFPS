using System;
using System.Collections.Generic;
using System.Text;

namespace TMTDentalFPS.APIInfo
{
    public class LoginResponse
    {
        public bool succeeded { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string config { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string userName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string companyId { get; set; }

    }
}
