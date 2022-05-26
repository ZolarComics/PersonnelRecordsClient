using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace PersonnelRecordsClient.MVVM
{
    public class User : INotifyPropertyChanged
    { 
        public int id { get; set; }

        public string login, pass, typeUser;
        
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
        const TypeUsers typeUserAdmin = TypeUsers.SimpleUser;
        const TypeUsers typeSimpleUser = TypeUsers.Admin;

        public event PropertyChangedEventHandler PropertyChanged;

        private ComboBoxItem selectedTag;

        public ComboBoxItem SelectedTag
        {
            get => selectedTag;
            set
            {
                selectedTag = value;
                ChangeText(selectedTag.Tag as string);

            }
        }

        private void ChangeText(string index)
        {
            switch (index) 
            {
                case "1":
                    EnterSimpleUserTypeUser(typeSimpleUser);
                    break;

                case "2":
                    EnterAdminTypeUser(typeUserAdmin);
                    break;
            }
        }

        public User()
        {
           
        }

        public User(string login, string pass, string typeUser) //
        {
            this.login = login;
            this.pass = pass;
           this.typeUser = typeUser;
        }

        private void EnterSimpleUserTypeUser(TypeUsers typeSimpleUser)
        {

        }

        private void EnterAdminTypeUser(TypeUsers typeUserAdmin)
        {

        }
       
    }

    public enum TypeUsers
    {
        SimpleUser = 1,
        Admin = 2,
    }

}
