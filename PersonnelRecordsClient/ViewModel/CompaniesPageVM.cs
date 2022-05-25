using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.Views.Windows.Companies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel
{
    internal class CompaniesPageVM : INotifyPropertyChanged// BaseViewModel
    {       
        public CustomCommand SearchCompany { get; set; }

        public CustomCommand GoToStaffingList { get; set; }
        public CustomCommand AddCompany { get; set; }
        public CustomCommand SaveCompany { get; set; }
        public CustomCommand RemoveCompany { get; set; }
        public CustomCommand TagCompany { get; set; }
        public CustomCommand SortCompany { get; set; }
        public CompanyApi selectedCompany { get; set; }
        public CompanyApi SelectedCompany 
        { 
            get => selectedCompany;
            set
            {
                selectedCompany = value ;
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
        //public ObservableCollection<CompanyApi> Companies { get; set; } = new ObservableCollection<CompanyApi>();
        public List<CompanyApi> Companies { get; set; }

        public CompaniesPageVM()
        {          
            AddCompany = new CustomCommand(() =>
            {
                Task.Run(Add);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            });
            //SortCompany = new CustomCommand(() =>
            //{
            //    Task.Run(SortGetCompanies);
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            //});
            SaveCompany = new CustomCommand(() =>
            {
                try
                {
                    Task.Run(Save);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            TagCompany = new CustomCommand(() =>
            {
                Task.Run(TagDelete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            });
            RemoveCompany = new CustomCommand(()=>
                {
                    Task.Run(Delete);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
                });           
            GoToStaffingList = new CustomCommand(() =>
            {
                StaffingList staffingList = new StaffingList();
                staffingList.Show();
            });

            Task.Run(GetCompanies);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async Task Add()
        {
            SelectedCompany = new CompanyApi();            
            var result = Api.PostAsync<CompanyApi>(SelectedCompany, "Company");
            await GetCompanies();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
        }
        public async Task Save()
        {
            var result = await Api.PutAsync<CompanyApi>(SelectedCompany, "Company");
            await GetCompanies();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
        }
        public async Task AddArchive()
        {
            SelectedArchive = new ArchiveApi { OneRecord = SelectedCompany.Name, TwoRecord = SelectedCompany.Owner};
                var result = Api.PostAsync(SelectedCompany, "Company");            
        }
        public async Task Delete()
        {
            var result = await Api.DeleteAsync<CompanyApi>(SelectedCompany, "Company");
            await GetCompanies();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
        }
        public async Task TagDelete()
        {
            SelectedCompany.IsRemuved = 1;
            var result = await Api.PutAsync<CompanyApi>(SelectedCompany, "Company");
            await GetCompanies();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
        }
        async Task GetCompanies()
        {
            try
            {                
                var result = await Api.GetListAsync<CompanyApi[]>("Company");
                Companies = new List<CompanyApi>(result);
                var companies = new List<CompanyApi>(Companies);
                foreach (var company in companies)
                    if(company.IsRemuved==1)
                        Companies.Remove(company);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
                 SignalChanged("Companies");
                
               
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }            
        }
        //public async Task SortGetCompanies()
        //{
        //    try
        //    {
        //        //string sort = Company.IsRemuved.ToString();
        //        var resultCompany = await Api.GetListAsync<CompanyApi[]>("Company");                
        //        var result = resultCompany.ToList().Where(u => u.Sort == searchText);
        //        Companies = new List<CompanyApi>(result);
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show($"{e}");
        //    }
        //}
        void SignalChanged([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
