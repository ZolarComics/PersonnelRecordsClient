using PersonnelRecordsClient.ViewModel.WindowsVM;
using System.Windows;

namespace PersonnelRecordsClient.Views.Windows.Workers
{
    /// <summary>
    /// Логика взаимодействия для EditWorker.xaml
    /// </summary>
    public partial class EditWorker : Window
    {
        public EditWorker()
        {
            InitializeComponent();
            DataContext = new WorkersEditVM(Dispatcher);
        }
    }
}
