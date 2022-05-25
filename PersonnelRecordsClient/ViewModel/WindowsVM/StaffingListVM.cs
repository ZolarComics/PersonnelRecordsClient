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

        public StaffingListVM()
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
                var workers = new List<WorkerApi>(Workers);
                foreach (var worker in workers)
                    if (worker.IsRemuved == 1)
                        workers.Remove(worker);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
                SignalChanged("Staffings");
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
                var staffings = new List<StaffingApi>(Staffings);
                foreach (var staffing in staffings)
                    if (staffing.IsRemuved == 1)
                        Staffings.Remove(staffing);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
                SignalChanged("Staffings");
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
