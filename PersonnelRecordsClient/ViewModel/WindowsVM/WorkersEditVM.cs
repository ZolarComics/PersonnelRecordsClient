using ModelApi;
using PersonnelRecordsClient.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel.WindowsVM
{
    public class WorkersEditVM : INotifyPropertyChanged
    {
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
        private ArchiveApi selectedArchive;
        public ArchiveApi SelectedArchive
        {
            get => selectedArchive;
            set
            {
                selectedArchive = value;
                SignalChanged();
            }
        }

        private Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public List<WorkerApi> Workers { get; set; }
        public List<WorkerApi> Archives { get; set; }
        public CustomCommand SaveWorker { get; set; }
        public CustomCommand AddWorker { get; set; }
        public CustomCommand RemoveWorker { get; set; }

        public WorkersEditVM(WorkerApi worker)
        {
            AddWorker = new CustomCommand(() =>
            {
                Task.Run(Add);
            });
            SaveWorker = new CustomCommand(() =>
            {
                Task.Run(AddArchive);
                Task.Run(Save);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            RemoveWorker = new CustomCommand(() =>
            {
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
        }
        public WorkersEditVM(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
        public async Task AddArchive()
        {
            string OldSurname = SelectedWorker.Surname;
            string Name = SelectedWorker.Name;
            string Phone = SelectedWorker.Phone;
            string Patronymic = SelectedWorker.Patronymic;
            string Email = SelectedWorker.Email;
            //string Patronymic = SelectedWorker.Patronymic;
           // var archive = new ArchiveApi { oldRecord =  }
            var result = Api.PostAsync<ArchiveApi>(SelectedArchive, "archive");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
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
        public async Task Delete()
        {
            var result = await Api.DeleteAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }


        public async Task GetWorkers()
        {
            var result = await Api.GetListAsync<WorkerApi[]>("Worker");
            Workers = new List<WorkerApi>(result);
            SignalChanged("Workers");
        }
    }
}