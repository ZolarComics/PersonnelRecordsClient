using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Windows.Companies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
    internal class CompaniesPageVM : INotifyPropertyChanged// BaseViewModel
    {
        //private readonly Dispatcher dispatcher;
        public CustomCommand GoToStaffingList { get; set; }
        public CompanyApi SelectedCompany { get; set; }
        //public ObservableCollection<CompanyApi> Companies { get; set; } = new ObservableCollection<CompanyApi>();
        public List<CompanyApi> Companies { get; set; }

        public CompaniesPageVM(Dispatcher dispatcher)
        {
            //this.dispatcher = dispatcher;
            GoToStaffingList = new CustomCommand(() =>
            {
                StaffingList staffingList = new StaffingList();
                staffingList.Show();
            });

            Task.Run(GetCompanies);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        async Task GetCompanies()
        {
            try
            {
                var result = await Api.GetListAsync<CompanyApi[]>("Company");
                Companies = new List<CompanyApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs( nameof(Companies)));
                //SignalChanged(nameof(Companies));
                /*dispatcher.Invoke(() => {
                    Companies.Clear();
                    foreach (var r in result)
                        Companies.Add(r);
                });*/

            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
    }
}
