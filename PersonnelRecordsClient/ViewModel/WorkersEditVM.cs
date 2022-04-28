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

namespace PersonnelRecordsClient.ViewModel
{
    public class WorkersEditVM : INotifyPropertyChanged
    {
        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                SignalChanged();
            }
        }


        private Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public List<WorkerApi> Workers { get; set; }
        public CustomCommand SaveWorker { get; set; }
        public WorkerApi AddWorker { get; set; }

        public WorkersEditVM(WorkerApi worker)
        {
            Task.Run(GetListWorkers);

            if (worker == null)
            {
                AddWorker = new WorkerApi();
            }
            else
            {
                AddWorker = new WorkerApi()
                {
                    Id = worker.Id
                };
            };

            SaveWorker = new CustomCommand(() =>
            {
                if (AddWorker.Id == 0)
                    Task.Run(CreateNewWorker);
                else
                    Task.Run(EditWorker);
            });
        }
        public WorkersEditVM(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async Task CreateNewWorker()
        {
            await Api.PostAsync<WorkerApi>(AddWorker, "Worker");
        }

        public async Task EditWorker()
        {
            await Api.PutAsync<WorkerApi>(AddWorker, "Worker");
        }

        public async Task GetListWorkers()
        {
            var result = await Api.GetListAsync<WorkerApi[]>("Worker");
            Workers = new List<WorkerApi>(result);
            SignalChanged("Workers");
        }
    }
}
