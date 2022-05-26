using PersonnelRecordsClient.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelRecordsClient.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для WorkersPage.xaml
    /// </summary>
    public partial class WorkersPage : Page
    {
        public WorkersPage()

        {
            InitializeComponent();
            DataContext = new WorkersPageVM();
            
        }

        public DateTime ExperienceTimeStart { get; set; }
        public DateTime ExperienceTimeEnd { get; set; }

        public TimeSpan ExperienceTimeCalculate { get; set; }

        private void Button_ClickTime(object sender, RoutedEventArgs e)
        {
            DateTime dateStart = new DateTime();
            dateStart = ExperienceTimeStart;

            DateTime dateEnd = new DateTime();
            dateEnd = ExperienceTimeEnd;

            TimeSpan diffCalculate = dateEnd.Subtract(dateStart);

            textBoxTime.Text = "опыт работы: " + String.Format(diffCalculate.TotalHours.ToString()) + " часов";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock1.Text = ListViewWorkers.Items.Count.ToString();
            string block = TextBlock1.Text;
        }
    }
}
