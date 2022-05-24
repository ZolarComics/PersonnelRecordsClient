using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelRecordsClient.MVVM
{
    public class User
    {
        public int id { get; set; }

        public string login, email, pass, typeUser;

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
        public string TypeUser
        {
            get { return typeUser; }
            set { typeUser = value; }
        }
        public User(){}


        public User(string login, string pass, string typeUser)
        {
            this.login = login;
            this.pass = pass;
            this.typeUser = typeUser;
        }

    }
    public enum TypeUsers
    {
        SimpleUser,
        AdmiUser
    }

}
