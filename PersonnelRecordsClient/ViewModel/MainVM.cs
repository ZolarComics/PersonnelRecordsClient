using PersonnelRecordsClient.AuthorizationPOP;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Pages;
using PersonnelRecordsClient.Views.Windows.Companies;
using PersonnelRecordsClient.Views.Windows.Companies.Staffing;
using PersonnelRecordsClient.Views.Windows.Workers;
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
        public CustomCommand GoStaffing { get; set; }
        public CustomCommand GoWorkerEdit { get; set; }
        public CustomCommand GoBasket { get; set; }
        public CustomCommand GoArchive { get; set; }
        public CustomCommand GoSearch { get; set; }
        public CustomCommand GoBasketPage { get; set; }
        public CustomCommand GoSetting { get; set; }
        

        public MainVM()
        {
            GoWorkerEdit = new CustomCommand(() =>
            {
                EditWorker EditWorker = new EditWorker();
                EditWorker.Show();
                //EditWorker = new EditWorker.Show();
                //MainWindow.MainNavigate(new EditWorker());
            });
            GoStaffing = new CustomCommand(() =>
            {
                //MainWindow.MainNavigate(new EditStaffing());
                StaffingList StaffingList = new StaffingList();
                StaffingList.Show();
            });
            GoBasket = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new CompaniesPage());
            });
            GoArchive = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new ArchivePage());
            });
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
               EducationList EducationList = new EducationList();
               EducationList.Show();
               // MainWindow.MainNavigate(new EducationList());
           });
            GoSetting = new CustomCommand(() =>
            {
                Auth AutWin = new Auth();
                AutWin.Show();
            });
            GoSearch = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new SearchPage());
            });
            GoBasketPage = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new BasketPage());
            });

        }
    }
}
