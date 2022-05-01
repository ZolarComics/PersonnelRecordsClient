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

namespace PersonnelRecordsClient.ViewModel
{
    class ArchivePageVM : INotifyPropertyChanged
    {
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
        private readonly Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        public CustomCommand RemoveArchiveMetod;

        // public ObservableCollection<WorkerApi> Workers { get; set; } = new ObservableCollection<WorkerApi>();
        public List<ArchiveApi> Archives { get; set; }

        public ArchivePageVM(Dispatcher dispatcher)
        {
            //this.dispatcher = dispatcher;

            Task.Run(GetArchive);
        }
       public ArchivePageVM()
        {
            RemoveArchiveMetod = new CustomCommand(() =>
                {
                    Task.Run(RemoveArchive);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
                });            
        }

        async Task GetArchive()
        {
            try
            {
                var result = await Api.GetListAsync<ArchiveApi[]>("Archive");
                Archives = new List<ArchiveApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));                
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        public async Task RemoveArchive()
        {
            var result = await Api.DeleteAsync<ArchiveApi>(SelectedArchive, "Archive");
            await Task.Run(RemoveArchive);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
            
        }
        void SignalChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
