using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Themes.Pages;
using PersonnelRecordsClient.Themes.Windows.Companies.Staffing;
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
        public CustomCommand GoEducation { get; set; }

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
            GoEducation = new CustomCommand(() =>
           {
               MainWindow.MainNavigate(new AppointWorker());
           });
        }
    }
}
