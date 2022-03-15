using ModelApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
    class ArchivePageVM : INotifyPropertyChanged
    {
        private readonly Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        public ArchiveApi SelectedArchive { get; set; }
        // public ObservableCollection<WorkerApi> Workers { get; set; } = new ObservableCollection<WorkerApi>();
        public List<ArchiveApi> Archives { get; set; }

        public ArchivePageVM(Dispatcher dispatcher)
        {
            //this.dispatcher = dispatcher;

            Task.Run(GetArchive);
        }

        async Task GetArchive()
        {
            try
            {
                var result = await Api.GetListAsync<ArchiveApi[]>("Archive");
                Archives = new List<ArchiveApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
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
