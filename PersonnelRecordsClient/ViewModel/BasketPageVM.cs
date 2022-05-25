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
    public class BasketPageVM : INotifyPropertyChanged
    {
        public List<CompanyApi> Companies { get; set; }
        public List<WorkerApi> Workers { get; set; }
        CustomCommand DeleteBasket { get; set; }
        private readonly Dispatcher dispatcher;
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
        public CompanyApi selectedCompany { get; set; }
        public CompanyApi SelectedCompany
        {
            get => selectedCompany;
            set
            {
                selectedCompany = value;
                SignalChanged();
            }
        }

        //CustomCommand  GetCompaniesIsRemuvedList { get;set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public BasketPageVM(Dispatcher dispatcher)
        {
            DeleteBasket = new CustomCommand(() =>
            {
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            });
            Task.Run(GetCompaniesIsRemuved);
            Task.Run(GetWorkersIsRemuved);
        }        

        async Task Delete()
        {
            var result = await Api.DeleteAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkersIsRemuved();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }
        async Task GetCompaniesIsRemuved()
        {
            try
            {
                var result = await Api.GetListAsync<CompanyApi[]>("Company");
                Companies = new List<CompanyApi>(result);
                var companies = new List<CompanyApi>(Companies);
                foreach (var company in companies)
                    if (company.IsRemuved != 1)
                        Companies.Remove(company);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
                SignalChanged("Companies");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        async Task GetWorkersIsRemuved()
        {
            try
            {
                var result = await Api.GetListAsync<WorkerApi[]>("Worker");
                Workers = new List<WorkerApi>(result);
                var workers = new List<WorkerApi>(Workers);
                foreach (var worker in workers)
                    if (worker.IsRemuved != 1)
                        Workers.Remove(worker);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
                SignalChanged("Workers");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }
        void SignalChanged([CallerMemberName] string prop = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}
