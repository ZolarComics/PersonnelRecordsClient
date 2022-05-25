using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Pages;
using PersonnelRecordsClient.AuthorizationPOP;
using System;
using System.Collections.Generic;
using System.Linq;
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
using PersonnelRecordsClient.ViewModel;

namespace PersonnelRecordsClient.AuthorizationPOP
{
    /// <summary>
    /// Логика взаимодействия для Aut.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
        public CustomCommand GoBasketPage { get; set; }

        private void GoProgram(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text.Trim();
            string pass = PassBox.Password.Trim();

            if (login.Length < 5)
            {
                TextBoxLogin.ToolTip = "В поле логина нужно больше 5 символов";
                TextBoxLogin.Background = Brushes.Red;

            }
            else if (pass.Length < 5)
            {
                PassBox.ToolTip = "В поле пароля нужно больше 5 символов";
                PassBox.Background = Brushes.Red;
            }

            else
            {
                TextBoxLogin.ToolTip = "";
                TextBoxLogin.Background = Brushes.Transparent;
                PassBox.ToolTip = "";
                PassBox.Background = Brushes.Transparent;
            }

            User authUser = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                authUser = db.Users.Where(b => b.Login == login && b.Pass == pass).FirstOrDefault();
            }

            if (authUser != null)
            {
                GoBasketPage = new CustomCommand(() =>
                    {
                       // MainWindow.MainNavigate(new BasketPage());
                        this.Close();
                    });
                  //MainWindow.MainNavigate(new BasketPage());                
                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("Неверный логин или пароль!");
        }

        private void GoRegistration(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }
    }
}