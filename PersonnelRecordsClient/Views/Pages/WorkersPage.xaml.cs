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
        public List<WorkerApi> Workers = new List<WorkerApi>();
        public WorkersPage()
        {
            InitializeComponent();
            DataContext = new WorkersPageVM(Dispatcher);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var worker in list.SelectedItems)
            {
                if (worker is WorkerApi)
                {
                    Workers.Add((WorkerApi)worker);
                }
            }
            DataContext = new WorkersPageVM(Workers);
        }
    }
}
