using PersonnelRecordsClient.ViewModel.WindowsVM;
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

namespace PersonnelRecordsClient.Views.Windows.Companies
{
    /// <summary>
    /// Логика взаимодействия для StaffingList.xaml
    /// </summary>
    public partial class StaffingList : Window
    {
        public StaffingList()
        {
            InitializeComponent();
            DataContext = new StaffingListVM();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock1.Text = ListViewStaffings.Items.Count.ToString();
            string block = TextBlock1.Text;
        }
    }
}
