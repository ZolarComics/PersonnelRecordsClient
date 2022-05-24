using ModelApi;
using PersonnelRecordsClient.MVVM;
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
    class ArchivePageVM : BaseViewModel, INotifyPropertyChanged
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
        public CustomCommand RemoveArchive { get; set; }
        public CustomCommand AddArchive { get; set; }

         public ObservableCollection<WorkerApi> Workers { get; set; } = new ObservableCollection<WorkerApi>();
        public List<ArchiveApi> Archives { get; set; }

        public ArchivePageVM(Dispatcher dispatcher)
        {
            //this.dispatcher = dispatcher;
            Task.Run(GetArchive);
        }
       public ArchivePageVM()
        {
            AddArchive = new CustomCommand(() =>
            {
                Task.Run(Add);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
            });
            RemoveArchive = new CustomCommand(() =>
                {
                    Task.Run(Remove);
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
        public async Task Add()
        {
           // this.SelectedArchive = ArchivePageVM(dispatcher);
        }
        public async Task Remove()
        {
            var result = await Api.DeleteAsync<ArchiveApi>(SelectedArchive, "Archive");
            await GetArchive();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
        }
        void SignalChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
