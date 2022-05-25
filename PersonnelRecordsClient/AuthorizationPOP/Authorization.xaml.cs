using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonnelRecordsClient.AuthorizationPOP
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        ApplicationContext db;

        //public List<User> UsersEnam = new List<User>();
        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                SignalChanged();
            }
        }

        public Authorization()
        {
            InitializeComponent();
            db = new ApplicationContext();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();
            string typeUser = TypeUserComboBox.Text.Trim();

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "Нужно более 5 симоволов в поле логина!";
                textBoxLogin.Background = Brushes.Red;
            }
            else if (pass.Length < 5)
            {
                passBox.ToolTip = "Нужно более 5 символов в поле пароля!";
                passBox.Background = Brushes.Red;
            }

            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;
                MessageBox.Show("Всё хорошо!");
                User user = new User(login, pass, typeUser);
                db.Users.Add(user);
                db.SaveChanges();
                Auth auth = new Auth();
                auth.Show();
                this.Close();
            }
        }
        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}

