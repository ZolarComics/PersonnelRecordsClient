using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelRecordsClient.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        
        public CustomCommand SearchCompany { get; set; }
        public List<CompanyApi> Companies { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchPage()
        {
            InitializeComponent();
            DataContext = new SearchPageVM(Dispatcher);
        }

        private void Button_ClickWorkers(object sender, RoutedEventArgs e)
        {
            TextBlockWorkers.Text = listViewWorkers.Items.Count.ToString();
            string block = TextBlockWorkers.Text;
        }
        private void Button_ClickArchives(object sender, RoutedEventArgs e)
        {
            TextBlockArchives.Text = listViewArchives.Items.Count.ToString();
            string block = TextBlockArchives.Text;
        }
        private void Button_ClickCompanies(object sender, RoutedEventArgs e)
        {
            TextBlockCompanies.Text = listViewCompanies.Items.Count.ToString();
            string block = TextBlockCompanies.Text;
        }
        private void Button_ClickStaffings(object sender, RoutedEventArgs e)
        {
            TextBlockStaffings.Text = listViewWorkers.Items.Count.ToString();
            string block = TextBlockStaffings.Text;
        }

    }
}
