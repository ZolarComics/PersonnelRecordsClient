using ModelApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
    class WorkersPageVM : INotifyPropertyChanged
    {
        private readonly Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        public WorkerApi SelectedWorker { get; set; }
        // public ObservableCollection<WorkerApi> Workers { get; set; } = new ObservableCollection<WorkerApi>();
        public List<WorkerApi> Workers { get; set; }

        public WorkersPageVM(Dispatcher dispatcher)
        {
            //this.dispatcher = dispatcher;

            Task.Run(GetWorkers);
        }

        async Task GetWorkers()
        {
            try
            {
                var result = await Api.GetListAsync<WorkerApi[]>("Worker");
                Workers = new List<WorkerApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
                //dispatcher.Invoke(() => {
                //    Workers.Clear();
                //    foreach (var r in result)
                //        Workers.Add(r);
                //});
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
    }
}
