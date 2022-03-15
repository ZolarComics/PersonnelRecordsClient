using ModelApi;
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

namespace PersonnelRecordsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow window;
        public MainWindow()
        {
            InitializeComponent();
            window = this;
            DataContext = new MainVM();
            //MainNavigate();

            //Test();
        }

        public static void MainNavigate(Page page)
        {
            window.Frame.Navigate(page);
        }

        async Task Test()
        {
            try
            {
                var result = await Api.GetAsync<CompanyApi>(1, "Company");
            }
            catch (Exception e)
            { }
        }
    }
}
