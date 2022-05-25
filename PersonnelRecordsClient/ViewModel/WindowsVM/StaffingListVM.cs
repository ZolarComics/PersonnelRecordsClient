using ModelApi;
using PersonnelRecordsClient.MVVM;
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
    class StaffingListVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public StaffingApi selectedStaffing { get; set; }
        public StaffingApi SelectedStaffing
        { get => selectedStaffing;
            set
            {
                selectedStaffing = value;
                SignalChanged();
            }
        }
        public List<StaffingApi> Staffings { get; set; }       
        public List<WorkerApi> Workers;    
        
        public CustomCommand AddStaffing { get; set; }        
        public CustomCommand SaveStaffing { get; set; }
        public CustomCommand RemoveStaffing { get; set; }

        public StaffingListVM(List<StaffingApi> Workers)
        {
            AddStaffing = new CustomCommand(() =>
            {
                Task.Run(Add);
            });
            SaveStaffing = new CustomCommand(() =>
            {
                Task.Run(Save);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
            });
            RemoveStaffing = new CustomCommand(() =>
            {
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
            });
        }
        public StaffingListVM()
        {
          Task.Run(GetStaffings);
          Task.Run(GetWorkers);
        }
        async Task Delete()
        {
            var result = Api.DeleteAsync<StaffingApi>(SelectedStaffing, "Staffing");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }
        async Task Add()
        {
            SelectedStaffing = new StaffingApi();
            var result = Api.PostAsync<StaffingApi>(SelectedStaffing, "Staffing");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }
        async Task Save()
        {
            //var oldWorker = SelectedWorker;
            var result = Api.PutAsync<StaffingApi>(SelectedStaffing, "Staffing");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }
        async Task GetWorkers()
        {
            try
            {
                var result = await Api.GetListAsync<WorkerApi[]>("Worker");
                Workers = new List<WorkerApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
                SignalChanged("Workers");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        async Task GetStaffings()
        {
            try
            {
                var result = await Api.GetListAsync<StaffingApi[]>("Staffing");
                Staffings = new List<StaffingApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));                
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }

        }
        void SignalChanged([CallerMemberName] string prop = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
