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
using System.Windows.Data;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
    public class ArchivePageVM : INotifyPropertyChanged
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

            StaffingsCollectionView = CollectionViewSource.GetDefaultView(Staffings);
            StaffingsCollectionView.Filter = FilterStaffings;
            StaffingsCollectionView.SortDescriptions.Add(new SortDescription(nameof(StaffingApi.Id), ListSortDirection.Ascending));

            WorkersCollectionView = CollectionViewSource.GetDefaultView(Workers);
            WorkersCollectionView.Filter = FilterWorkers;
            WorkersCollectionView.SortDescriptions.Add(new SortDescription(nameof(WorkerApi.Name), ListSortDirection.Ascending));

            ImpactTypesCollectionView = CollectionViewSource.GetDefaultView(ImpactTypes);
            ImpactTypesCollectionView.Filter = FilterImpactType;
            ImpactTypesCollectionView.SortDescriptions.Add(new SortDescription(nameof(ImpactTypeApi.Id), ListSortDirection.Ascending));

        }
        #region
        // Staffings
        public ObservableCollection<StaffingApi> Staffings { get; set; }
        public ICollectionView StaffingsCollectionView { get; set; }

        private string _staffingIdFilterFilter = string.Empty;
        public string StaffingIdFilter
        {
            get
            {
                return _staffingIdFilterFilter;
            }
            set
            {
                _staffingIdFilterFilter = value;
                OnPropertyChanged(nameof(WorkerNameFilter));
                StaffingsCollectionView.Refresh();
            }
        }
        private bool FilterStaffings(object obj)
        {
            if (obj is StaffingApi staffing)
            {
                return staffing.Id.ToString().Contains(StaffingIdFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        // Workers
        public ICollectionView WorkersCollectionView { get; set; }
        public ObservableCollection<WorkerApi> Workers { get; set; }

        private string _workerNameFilter = string.Empty;
        public string WorkerNameFilter
        {
            get
            {
                return _workerNameFilter;
            }
            set
            {
                _workerNameFilter = value;
                OnPropertyChanged(nameof(WorkerNameFilter));
                WorkersCollectionView.Refresh();
            }
        }
        private bool FilterWorkers(object obj)
        {
            if (obj is WorkerApi worker)
            {
                return worker.Name.Contains(WorkerNameFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        // ImpactType
        public ICollectionView ImpactTypesCollectionView { get; set; }
        public ObservableCollection<ImpactTypeApi> ImpactTypes { get; set; }
        private string _impactTypeFilter = string.Empty;
        public string ImpactTypeFilter
        {
            get
            {
                return _impactTypeFilter;
            }
            set
            {
                _impactTypeFilter = value;
                OnPropertyChanged(nameof(ImpactTypeFilter));
                ImpactTypesCollectionView.Refresh();
            }
        }

        private bool FilterImpactType(object obj)
        {
            if (obj is ImpactTypeApi impactType)
            {
                return impactType.Id.ToString().Contains(ImpactTypeFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
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
