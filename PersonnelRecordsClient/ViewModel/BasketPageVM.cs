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
        private readonly Dispatcher dispatcher;
       /* public CompanyIsRemuvedApi selectedIsRemuved { get; set; }
        public CompanyIsRemuvedApi SelectedIsRemuved
        {
            get => selectedIsRemuved;
            set
            {
                selectedIsRemuved = value;
                SignalChanged();
            }
        }*/

        //CustomCommand  GetCompaniesIsRemuvedList { get;set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public BasketPageVM(Dispatcher dispatcher)
        {
            Task.Run(GetCompaniesIsRemuved);
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
        void SignalChanged([CallerMemberName] string prop = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}
