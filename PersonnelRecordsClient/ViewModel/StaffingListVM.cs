using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Windows.Companies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel.WindowsVM
{
    public class StaffingListVM : INotifyPropertyChanged
    {
        public StaffingApi selectedCompany { get; set; }

        private StaffingApi selectedStaffing;
        public StaffingApi SelectedStaffing
        {
            get => selectedStaffing;
            set
            {
                selectedStaffing = value;
                SignalChanged();
            }
        }

        private List<StaffingApi> staffings;
        public List<StaffingApi> Staffings
        {
            get => staffings;
            set
            {
                staffings = value;
                SignalChanged();
            }
        }

        public CustomCommand CreateStaffing { get; set; }
        public CustomCommand EditStaffing { get; set; }
        public CustomCommand RemoveStaffing { get; set; }


        public StaffingListVM(Dispatcher dispatcher)
        {
            CreateStaffing = new CustomCommand(() =>
            {
                if (SelectedStaffing == null)
                    return;

                MessageBoxResult result = MessageBox.Show("Создать новый?", "Или нет",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    Task.Run(Add);
                }

                else if (result == MessageBoxResult.No)
                {
                    var staffing = new StaffingApi { Position = "example", Note = "example", Salary = 1231123 };
                    Task<int> task = Api.PostAsync<StaffingApi>(staffing, "Staffing");
                    SelectedStaffing = staffing;
                }
                Task.Run(GetStaffings);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));

            });

            EditStaffing = new CustomCommand(() =>
            {
                StaffingList staffinglist = new StaffingList(); //передать selectedStaffing
                staffinglist.Show();
            });

            RemoveStaffing = new CustomCommand(() =>
            {
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
            });
        }

        public async Task Add()
        {

            SelectedStaffing = new StaffingApi();
            var result = Api.PostAsync<StaffingApi>(SelectedStaffing, "Staffing");

            StaffingList staffinglist = new StaffingList();
            staffinglist.Show();

            await GetStaffings();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }


        public async Task Save()
        {
            var result = await Api.PutAsync<StaffingApi>(SelectedStaffing, "Staffing");
            await GetStaffings();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }
        public async Task Delete()
        {
            var result = await Api.DeleteAsync<StaffingApi>(SelectedStaffing, "Staffing");
            await GetStaffings();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }
        async Task GetStaffings()
        {
            try
            {
                var result = await Api.GetListAsync<StaffingApi[]>("Staffing");
                Staffings = new List<StaffingApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
                SignalChanged("Staffings");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
