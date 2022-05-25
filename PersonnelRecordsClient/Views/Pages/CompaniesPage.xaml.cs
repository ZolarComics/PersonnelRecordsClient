﻿using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelRecordsClient.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для CompaniesPage.xaml
    /// </summary>
    public partial class CompaniesPage : Page
    {
        public CustomCommand SearchCompany { get; set; }
        public List<CompanyApi> Companies { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        string SearchText;
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

        public CompaniesPage()
        {
            InitializeComponent();
           SearchText = searchText.Text.Trim();
            DataContext = new CompaniesPageVM();
            SearchCompany = new CustomCommand(() =>
            {
                Task.Run(Search);
               // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            });
        }
        public async Task Search()
        {
            //if (SelectedCompany.Name == SearchText)
            //    Companies = Companies.FindAll(x => x.Name == "SearchText");
            var result = await Api.GetListAsync<CompanyApi[]>("Company");
            Companies = new List<CompanyApi>(result);
            var companies = new List<CompanyApi>(Companies);
            foreach (var company in companies)
                if (company.Name == SearchText)
                    Companies.Remove(company);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            SignalChanged("Companies");
            //await GetCompanies();
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));          
        }
        async Task GetCompanies()
        {
            try
            {
                var result = await Api.GetListAsync<CompanyApi[]>("Company");
                Companies = new List<CompanyApi>(result);
                var companies = new List<CompanyApi>(Companies);
                foreach (var company in companies)
                    if (company.Name == SearchText)
                        Companies.Remove(company);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
                SignalChanged("Companies");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }        
        //public void LoadCompany(List<CompanyApi> companies)
        //{
        //    Companies.Clear(); // очищаем лист с элементами

        //    for (int i = 0; i < companies.Count; i++) // перебираем элементы
        //    {
        //        Companies.Add(companies[i]); // добавляем элементы в ListBox
        //    }
        //}
        void SignalChanged([CallerMemberName] string prop = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
