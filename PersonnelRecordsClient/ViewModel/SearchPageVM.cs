﻿using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Windows.Companies;
using PersonnelRecordsClient.Views.Windows.Companies.Staffing;
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
    public class SearchPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dispatcher dispatcher;

        public CustomCommand GoToStaffingList { get; set; }
        public CustomCommand RemoveCompany { get; set; }

        public CustomCommand EditWorker { get; set; }
        public CustomCommand DeleteWorker { get; set; }
        public CustomCommand GoStaffing { get; set; }

        public CustomCommand RemoveStaffing { get; set; }

        public CustomCommand RemoveArchive { get; set; }
        public CustomCommand AddArchive { get; set; }

        public SearchPageVM(Dispatcher dispatcher)
        {
            
            Task.Run(GetArchive);
            Task.Run(GetWorkers);
            Task.Run(GetCompanies);
            Task.Run(GetStaffings);
        }
        public SearchPageVM()
        {
            //commands
            DeleteWorker = new CustomCommand(() =>
            {
                Task.Run(DeleteWorkers);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            GoStaffing = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new AppointWorker());
            });

            RemoveArchive = new CustomCommand(() =>
            {
                Task.Run(RemoveArchives);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
            });

            RemoveStaffing = new CustomCommand(() =>
            {
                Task.Run(DeleteStaffings);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
            });

            RemoveCompany = new CustomCommand(() =>
            {
                Task.Run(DeleteCompanies);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            });
            GoToStaffingList = new CustomCommand(() =>
            {
                StaffingList staffingList = new StaffingList();
                staffingList.Show();
            });
           
            //CollectionViews
            #region
            // staffings page
            StaffingsCollectionView = CollectionViewSource.GetDefaultView(Staffings);
            StaffingsCollectionView.Filter = FilterStaffings;
            StaffingsCollectionView.SortDescriptions.Add(new SortDescription(nameof(StaffingApi.Id), ListSortDirection.Ascending));

            // companies page
            CompaniesCollectionView = CollectionViewSource.GetDefaultView(Workers);
            CompaniesCollectionView.Filter = FilterCompanies;
            CompaniesCollectionView.SortDescriptions.Add(new SortDescription(nameof(CompanyApi.Name), ListSortDirection.Ascending));

            // archives page
            ArchiveCollectionView = CollectionViewSource.GetDefaultView(Archives);
            ArchiveCollectionView.Filter = FilterArchive;
            ArchiveCollectionView.SortDescriptions.Add(new SortDescription(nameof(ArchiveApi.Id), ListSortDirection.Ascending));

            // workers page
            WorkersCollectionView = CollectionViewSource.GetDefaultView(Workers);
            WorkersCollectionView.Filter = FilterWorkers;
            WorkersCollectionView.SortDescriptions.Add(new SortDescription(nameof(WorkerApi.Name), ListSortDirection.Ascending));
            //
            #endregion
        }

        //search staffingspage
        #region
        private StaffingApi selectedStaffing;
        public StaffingApi SelectedStaffing
        {
            get => selectedStaffing;
            set
            {
                selectedStaffing = value;
                SignalChanged();
            }
        }

        private List<StaffingApi> staffings;
        public List<StaffingApi> Staffings
        {
            get => staffings;
            set
            {
                staffings = value;
                SignalChanged();
            }
        }
        public ObservableCollection<StaffingApi> StaffingsCollection { get; set; }
        public ICollectionView StaffingsCollectionView { get; set; }

        private string _staffingIdFilter = string.Empty;
        public string StaffingIdsFilter
        {
            get
            {
                return _staffingIdFilter;
            }
            set
            {
                _staffingIdFilter = value;
                OnPropertyChanged(nameof(StaffingIdsFilter));
                StaffingsCollectionView.Refresh();
            }
        }

        private string _staffingPositionFilter = string.Empty;
        public string StaffingPositionsFilter
        {
            get
            {
                return _staffingPositionFilter;
            }
            set
            {
                _staffingPositionFilter = value;
                OnPropertyChanged(nameof(StaffingPositionsFilter));
                StaffingsCollectionView.Refresh();
            }
        }

        private string _staffingSalaryFilter = string.Empty;
        public string StaffingSalarysFilter
        {
            get
            {
                return _staffingSalaryFilter;
            }
            set
            {
                _staffingSalaryFilter = value;
                OnPropertyChanged(nameof(StaffingSalarysFilter));
                StaffingsCollectionView.Refresh();
            }
        }

        private string _staffingWorkerFilter = string.Empty;
        public string StaffingWorkersIdFilter
        {
            get
            {
                return _staffingWorkerFilter;
            }
            set
            {
                _staffingWorkerFilter = value;
                OnPropertyChanged(nameof(StaffingWorkersIdFilter));
                StaffingsCollectionView.Refresh();
            }
        }

        private string _staffingCompanyFilter = string.Empty;
        public string StaffingCompaniesIdFilter
        {
            get
            {
                return _staffingCompanyFilter;
            }
            set
            {
                _staffingCompanyFilter = value;
                OnPropertyChanged(nameof(StaffingCompaniesIdFilter));
                StaffingsCollectionView.Refresh();
            }
        }
        private bool FilterStaffings(object obj)
        {
            if (obj is StaffingApi staffing)
            {
                return staffing.Id.ToString().Contains(StaffingIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                    staffing.Id.ToString().Contains(StaffingPositionsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                    staffing.Id.ToString().Contains(StaffingSalarysFilter, StringComparison.InvariantCultureIgnoreCase) &&
                    staffing.Id.ToString().Contains(StaffingWorkersIdFilter, StringComparison.InvariantCultureIgnoreCase) &&
                    staffing.Id.ToString().Contains(StaffingCompaniesIdFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        public async Task DeleteStaffings()
        {
            var result = await Api.DeleteAsync<StaffingApi>(SelectedStaffing, "Staffing");
            await GetStaffings();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
        }

        async Task GetStaffings()
        {
            try
            {
                var result = await Api.GetListAsync<StaffingApi[]>("Staffing");
                Staffings = new List<StaffingApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));
                SignalChanged("Staffings");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        #endregion


        //search archivespage
        #region
        public ICollectionView ArchiveCollectionView { get; set; }

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

        private List<ArchiveApi> archives;
        public List<ArchiveApi> Archives
        {
            get => archives;
            set
            {
                archives = value;
                SignalChanged();
            }
        }
        public ObservableCollection<ArchiveApi> ArchivesCollection { get; set; }

        private string _archiveStaffingIdFilter = string.Empty;
        public string ArchiveStaffingIdsFilter
        {
            get
            {
                return _archiveStaffingIdFilter;
            }
            set
            {
                _archiveStaffingIdFilter = value;
                OnPropertyChanged(nameof(ArchiveStaffingIdsFilter));
                StaffingsCollectionView.Refresh();
            }
        }

        private string _archiveWorkerIdFilter = string.Empty;
        public string ArchiveWorkerIdsFilter
        {
            get
            {
                return _archiveWorkerIdFilter;
            }
            set
            {
                _archiveWorkerIdFilter = value;
                OnPropertyChanged(nameof(ArchiveWorkerIdsFilter));
                StaffingsCollectionView.Refresh();
            }
        }

        private string _archiveWorkerNameFilter = string.Empty;
        public string ArchiveWorkerNamesFilter
        {
            get
            {
                return _archiveWorkerNameFilter;
            }
            set
            {
                _archiveWorkerNameFilter = value;
                OnPropertyChanged(nameof(ArchiveWorkerNamesFilter));
                StaffingsCollectionView.Refresh();
            }
        }
        private string _archiveImpactTypeFilter = string.Empty;
        public string ArchiveImpactTypesFilter
        {
            get
            {
                return _archiveImpactTypeFilter;
            }
            set
            {
                _archiveImpactTypeFilter = value;
                OnPropertyChanged(nameof(ArchiveImpactTypesFilter));
                StaffingsCollectionView.Refresh();
            }
        }
        private bool FilterArchive(object obj)
        {
            if (obj is ArchiveApi archive)
            {
                return archive.StaffingId.ToString().Contains(ArchiveStaffingIdsFilter, StringComparison.InvariantCultureIgnoreCase)&&
                archive.WorkerId.ToString().Contains(ArchiveWorkerIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                archive.Id.ToString().Contains(ArchiveWorkerNamesFilter, StringComparison.InvariantCultureIgnoreCase) &&
                archive.Id.ToString().Contains(ArchiveImpactTypesFilter, StringComparison.InvariantCultureIgnoreCase);

            }
            return false;
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
        public async Task RemoveArchives()
        {
            var result = await Api.DeleteAsync<ArchiveApi>(SelectedArchive, "Archive");
            await GetArchive();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));

        }

        //
        #endregion


        //search workerspage
        #region
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

        public ICollectionView WorkersCollectionView { get; set; }
        public ObservableCollection<WorkerApi> WorkersCollection { get; set; }

        private string _workerIdFilter = string.Empty;
        public string WorkerIdsFilter
        {
            get
            {
                return _workerIdFilter;
            }
            set
            {
                _workerIdFilter = value;
                OnPropertyChanged(nameof(WorkerIdsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerNamesFilter = string.Empty;
        public string WorkerNamesFilter
        {
            get
            {
                return _workerNamesFilter;
            }
            set
            {
                _workerNamesFilter = value;
                OnPropertyChanged(nameof(WorkerNamesFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerPatronymicsFilter = string.Empty;
        public string WorkerPatronymicsFilter
        {
            get
            {
                return _workerPatronymicsFilter;
            }
            set
            {
                _workerPatronymicsFilter = value;
                OnPropertyChanged(nameof(WorkerPatronymicsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerPhonesFilter = string.Empty;
        public string WorkerPhonesFilter
        {
            get
            {
                return _workerPhonesFilter;
            }
            set
            {
                _workerPhonesFilter = value;
                OnPropertyChanged(nameof(WorkerPhonesFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerEmailsFilter = string.Empty;
        public string WorkerEmailsFilter
        {
            get
            {
                return _workerEmailsFilter;
            }
            set
            {
                _workerEmailsFilter = value;
                OnPropertyChanged(nameof(WorkerEmailsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerPassportIdsFilter = string.Empty;
        public string WorkerPassportIdsFilter
        {
            get
            {
                return _workerPassportIdsFilter;
            }
            set
            {
                _workerPassportIdsFilter = value;
                OnPropertyChanged(nameof(WorkerPassportIdsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerEducationIdsFilter = string.Empty;
        public string WorkerEducationIdsFilter
        {
            get
            {
                return _workerEducationIdsFilter;
            }
            set
            {
                _workerEducationIdsFilter = value;
                OnPropertyChanged(nameof(WorkerEducationIdsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerExperienceIdsFilter = string.Empty;
        public string WorkerExperienceIdsFilter
        {
            get
            {
                return _workerExperienceIdsFilter;
            }
            set
            {
                _workerExperienceIdsFilter = value;
                OnPropertyChanged(nameof(WorkerExperienceIdsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _workerSureNamesFilter = string.Empty;
        public string WorkerSureNamesFilter
        {
            get
            {
                return _workerSureNamesFilter;
            }
            set
            {
                _workerSureNamesFilter = value;
                OnPropertyChanged(nameof(WorkerSureNamesFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private bool FilterWorkers(object obj)
        {
            if (obj is WorkerApi worker)
            {
                return worker.Id.ToString().Contains(WorkerIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                worker.Name.Contains(WorkerNamesFilter, StringComparison.InvariantCultureIgnoreCase) &&
                worker.PassportId.ToString().Contains(WorkerPassportIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                worker.Patronymic.Contains(WorkerPatronymicsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                worker.Phone.Contains(WorkerPhonesFilter, StringComparison.InvariantCultureIgnoreCase) &&
                worker.EducationId.ToString().Contains(WorkerEducationIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                worker.Email.Contains(WorkerEmailsFilter, StringComparison.InvariantCultureIgnoreCase);
                //worker.ExperienceId.ToString().Contains(WorkerExperienceIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                //worker.SureName.Contains(WorkerSureNamesFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        public async Task DeleteWorkers()
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
        //
        #endregion


        //search companiespage
        #region 
        private CompanyApi selectedCompany;
        public CompanyApi SelectedCompany
        {
            get => selectedCompany;
            set
            {
                selectedCompany = value;
                SignalChanged();
            }
        }

        private List<CompanyApi> companies;
        public List<CompanyApi> Companies
        {
            get => companies;
            set
            {
                companies = value;
                SignalChanged();
            }
        }
        public ObservableCollection<CompanyApi> CompaniesCollection { get; set; }
        public List<CompanyApi> CompaniesList { get; set; }
        public ICollectionView CompaniesCollectionView { get; set; }

        private string _companyIdFilter = string.Empty;
        public string CompanyIdsFilter
        {
            get
            {
                return _companyIdFilter;
            }
            set
            {
                _companyIdFilter = value;
                OnPropertyChanged(nameof(CompanyIdsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _companyNameFilter = string.Empty;
        public string CompanyNamesFilter
        {
            get
            {
                return _companyNameFilter;
            }
            set
            {
                _companyNameFilter = value;
                OnPropertyChanged(nameof(CompanyNamesFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _companyAdressFilter = string.Empty;
        public string CompanyAdressesFilter
        {
            get
            {
                return _companyAdressFilter;
            }
            set
            {
                _companyAdressFilter = value;
                OnPropertyChanged(nameof(CompanyAdressesFilter));
                WorkersCollectionView.Refresh();
            }
        }


        private string _companyPhoneFilter = string.Empty;
        public string CompanyPhonesFilter
        {
            get
            {
                return _companyPhoneFilter;
            }
            set
            {
                _companyPhoneFilter = value;
                OnPropertyChanged(nameof(CompanyPhonesFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private string _companyDescriptionFilter = string.Empty;
        public string CompaniesDescriptionsFilter
        {
            get
            {
                return _companyDescriptionFilter;
            }
            set
            {
                _companyDescriptionFilter = value;
                OnPropertyChanged(nameof(CompaniesDescriptionsFilter));
                WorkersCollectionView.Refresh();
            }
        }

        private bool FilterCompanies(object obj)
        {
            if (obj is CompanyApi company)
            {
                return company.Id.ToString().Contains(CompanyIdsFilter, StringComparison.InvariantCultureIgnoreCase) &&
                company.Name.Contains(CompanyNamesFilter, StringComparison.InvariantCultureIgnoreCase) &&
                company.Adress.Contains(CompanyAdressesFilter, StringComparison.InvariantCultureIgnoreCase) &&
                company.Phone.Contains(CompanyPhonesFilter, StringComparison.InvariantCultureIgnoreCase) &&
                company.Description.Contains(CompaniesDescriptionsFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        public async Task DeleteCompanies()
        {
            var result = await Api.DeleteAsync<CompanyApi>(SelectedCompany, "Company");
            await GetCompanies();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
        }
        async Task GetCompanies()
        {
            try
            {
                var result = await Api.GetListAsync<CompanyApi[]>("Company");
                Companies = new List<CompanyApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }

        }
        #endregion

        public async Task LoadEntities()
        {
            var result = await Api.GetListAsync<WorkerApi>("Worker");
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        void SignalChanged([CallerMemberName] string prop = null) =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}