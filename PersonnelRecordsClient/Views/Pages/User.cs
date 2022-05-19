using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelRecordsClient.Themes.Pages
{
        public class User
    {
        public int id { get; set; }

        public string login, email, pass;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }   

        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }
        public User() { }


        public User(string login, string pass)
        {
            this.login = login;
            this.pass = pass;
        }
    }
}
