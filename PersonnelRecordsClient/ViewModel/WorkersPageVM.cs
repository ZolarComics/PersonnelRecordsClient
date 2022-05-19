using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.ViewModel.WindowsVM;
using PersonnelRecordsClient.Themes.Windows.Companies.Staffing;
using PersonnelRecordsClient.Themes.Windows.Workers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
   internal class WorkersPageVM : BaseViewModel, INotifyPropertyChanged
    {
        private  Dispatcher dispatcher;       
        public event PropertyChangedEventHandler PropertyChanged;
       
        private WorkerApi selectedWorker;
        public WorkerApi SelectedWorker
        {
            get => selectedWorker;
            set
            {
                selectedWorker = value;
                SignalChanged();
            }
        }

        // private List<WorkerApi> workers;
        public List<WorkerApi> Workers { get; set; } 

        public CustomCommand AddWorker { get; set; }
        public CustomCommand EditWorker { get; set; }
        public CustomCommand SaveWorker { get; set; }
        public CustomCommand RemoveWorker { get; set; }
        public CustomCommand GoStaffing { get; set; }




        public WorkersPageVM(List<WorkerApi> Workers)
        {
            AddWorker = new CustomCommand(() =>
           {
               Task.Run(Add);
           });
            SaveWorker = new CustomCommand(() =>
           {
               Task.Run(Save);
               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
           });           
            RemoveWorker = new CustomCommand(() =>
            {
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));                
            });
            GoStaffing = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new AppointWorker());
            });
        }
        public WorkersPageVM(Dispatcher dispatcher)
        {
            Task.Run(GetWorkers);
        }
        public async Task Add()
        {
            SelectedWorker = new WorkerApi();
            var result = Api.PostAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }
        public async Task Save()
        {
           var oldWorker = SelectedWorker;
            var result = await Api.PutAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }

        public async  Task Delete()
        {
            var result = await Api.DeleteAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
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

        public async Task LoadEntities()
        {
            var result = await Api.GetListAsync<WorkerApi>("Worker");
        }
        void SignalChanged([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}