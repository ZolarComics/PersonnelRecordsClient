using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Windows.Workers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
    class WorkersPageVM : INotifyPropertyChanged
    {
        private readonly Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ICollectionView WorkersCollectionView { get; set; }

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

        private List<WorkerApi> workers;
        public List<WorkerApi> Workers
        {
            get => workers;
            set
            {
                workers = value;
                SignalChanged();
            }
        }
        public CustomCommand CreateWorker { get; set; }
        public CustomCommand EditWorker { get; set; }
        public CustomCommand DeleteWorker { get; set; }

        public WorkersPageVM(List<WorkerApi> Workers) : this()
        {
            if (SelectedWorker == null)
                return;
            MessageBoxResult result = MessageBox.Show("Удалить?", "Да?",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var worker in Workers)
                    {
                        DeleteWorkers(worker);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public WorkersPageVM()
        {
            CreateWorker = new CustomCommand(() =>
            {
                EditWorker editWorker = new EditWorker();
                editWorker.ShowDialog();
            });

            EditWorker = new CustomCommand(() =>
            {
                if (SelectedWorker == null)
                    return;
                EditWorker editWorker = new EditWorker(SelectedWorker);
                editWorker.ShowDialog();
            });

            WorkersCollectionView = CollectionViewSource.GetDefaultView(Workers);
            WorkersCollectionView.Filter = FilterWorkers;
            WorkersCollectionView.SortDescriptions.Add(new SortDescription(nameof(WorkerApi.Name), ListSortDirection.Ascending));
        }
        public WorkersPageVM(Dispatcher dispatcher)
        {
            Task.Run(GetWorkers);
        }

        async void DeleteWorkers(WorkerApi worker)
        {
            var id = await Api.DeleteAsync<WorkerApi>(worker, "Worker");
        }

        async Task GetWorkers()
        {
            try
            {
                var result = await Api.GetListAsync<WorkerApi[]>("Worker");
                Workers = new List<WorkerApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
                /* dispatcher.Invoke(() => {
                Workers.Clear();
                foreach (var r in result)
                Workers.Add(r);
                });*/
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

        private bool FilterWorkers(object obj)
        {
            if (obj is WorkerApi worker)
            {
                return worker.Name.Contains(WorkersFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        //
        private string _workersFilter = string.Empty;
        public string WorkersFilter
        {
            get
            {
                return _workersFilter;
            }
            set
            {
                _workersFilter = value;
                OnPropertyChanged(nameof(WorkersFilter));
                WorkersCollectionView.Refresh();
            }
        }
        //
        private string _otherFilter = string.Empty;
        public string OtherFilter
        {
            get
            {
                return _otherFilter;
            }
            set
            {
                _otherFilter = value;
                OnPropertyChanged(nameof(OtherFilter));
                WorkersCollectionView.Refresh();
            }
        }
        //
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}