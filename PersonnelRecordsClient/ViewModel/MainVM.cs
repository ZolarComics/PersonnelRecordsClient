using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelRecordsClient.ViewModel
{
    class MainVM : BaseViewModel
    {
        public CustomCommand GoToCompanies { get; set; }
        public CustomCommand GoToWorkers { get; set; }
        public CustomCommand GoToArchives { get; set; }

        public MainVM()
        {
            GoToCompanies = new CustomCommand (() => 
            {
                MainWindow.MainNavigate(new CompaniesPage());
            });
            GoToWorkers = new CustomCommand(() => 
            {
            MainWindow.MainNavigate(new WorkersPage());
            });
            GoToArchives = new CustomCommand(() => 
            {
            MainWindow.MainNavigate(new ArchivePage());
            });
        }
    }
}
