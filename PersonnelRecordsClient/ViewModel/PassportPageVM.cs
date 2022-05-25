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

namespace PersonnelRecordsClient.ViewModel
{
    internal class PassportPageVM : INotifyPropertyChanged
    { 
   public CustomCommand SearchPassport { get; set; }
    public CustomCommand AddPassport { get; set; }
    public CustomCommand SavePassport { get; set; }
    public CustomCommand DeletePassport { get; set; }
    public CustomCommand TagPassport { get; set; }
    public CustomCommand SortPassport { get; set; }
    public PassportApi selectedPassport { get; set; }
    public PassportApi SelectedPassport
        {
        get => selectedPassport;
        set
        {
                selectedPassport = value;
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

    public List<PassportApi> Passports { get; set; }
    public List<WorkerApi> Archives { get; set; }

    public PassportPageVM()
    {
            AddPassport = new CustomCommand(() =>
        {
            Task.Run(Add);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
        });
            //SortCompany = new CustomCommand(() =>
            //{
            //    Task.Run(SortGetCompanies);
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            //});
            SavePassport = new CustomCommand(() =>
        {
            try
            {
                Task.Run(Save);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        });
            TagPassport = new CustomCommand(() =>
        {
            Task.Run(TagDelete);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
        });
            DeletePassport = new CustomCommand(() =>
        {
            Task.Run(Delete);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
        });

        Task.Run(GetPassports);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public async Task Add()
    {
            SelectedPassport = new PassportApi();
        var result = Api.PostAsync<PassportApi>(SelectedPassport, "Passport");
        await GetPassports();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
    }
    public async Task Save()
    {
        var result = await Api.PutAsync<PassportApi>(SelectedPassport, "Passport");
        await GetPassports();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
    }
    public async Task AddArchive()
    {
        SelectedArchive = new ArchiveApi { OneRecord = SelectedPassport.Series.ToString(), TwoRecord = SelectedPassport.Number.ToString() };
        var result = Api.PostAsync(SelectedArchive, "SelectedArchive");
    }
    public async Task Delete()
    {
        var result = await Api.DeleteAsync<PassportApi>(SelectedPassport, "Passport");
        await GetPassports();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
    }
    public async Task TagDelete()
    {
            SelectedPassport.IsRemuved = 1;
        var result = await Api.PutAsync<PassportApi>(SelectedPassport, "Passport");
        await GetPassports();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
    }
    async Task GetPassports()
    {
        try
        {
            var result = await Api.GetListAsync<PassportApi[]>("Passport");
                Passports = new List<PassportApi>(result);
            var educations = new List<PassportApi>(Passports);
            foreach (var education in educations)
                if (education.IsRemuved == 1)
                        Passports.Remove(education);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passports)));
            SignalChanged("Passports");


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