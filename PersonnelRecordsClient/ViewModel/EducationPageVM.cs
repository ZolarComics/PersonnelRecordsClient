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
    internal class EducationPageVM : INotifyPropertyChanged
    { 
    public CustomCommand SearchEducation { get; set; }   
    public CustomCommand AddEducation { get; set; }
    public CustomCommand SaveEducation { get; set; }
    public CustomCommand DeleteEducation { get; set; }
    public CustomCommand TagEducation { get; set; }
    public CustomCommand SortEducation { get; set; }
    public EducationApi selectedEducation { get; set; }
    public EducationApi SelectedEducation
        {
        get => selectedEducation;
        set
        {
            selectedEducation = value;
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

        public List<EducationApi> Educations { get; set; }
        public List<WorkerApi> Archives { get; set; }

        public EducationPageVM()
    {
            AddEducation = new CustomCommand(() =>
        {
            Task.Run(Add);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
        });
            //SortCompany = new CustomCommand(() =>
            //{
            //    Task.Run(SortGetCompanies);
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
            //});
            SaveEducation = new CustomCommand(() =>
        {
            try
            {
                Task.Run(Save);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        });
            TagEducation = new CustomCommand(() =>
        {
            Task.Run(TagDelete);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
        });
            DeleteEducation = new CustomCommand(() =>
        {
            Task.Run(Delete);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
        });       

        Task.Run(GetEducations);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public async Task Add()
    {
            SelectedEducation = new EducationApi();
        var result = Api.PostAsync<EducationApi>(SelectedEducation, "Education");
        await GetEducations();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
    }
    public async Task Save()
    {
        var result = await Api.PutAsync<EducationApi>(SelectedEducation, "Education");
        await GetEducations();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
    }
    public async Task AddArchive()
    {
        SelectedArchive = new ArchiveApi { OneRecord = SelectedEducation.EducationalPlace, TwoRecord = SelectedEducation.Speciality };
        var result = Api.PostAsync(SelectedArchive, "SelectedArchive");
    }
    public async Task Delete()
    {
        var result = await Api.DeleteAsync<EducationApi>(SelectedEducation, "Education");
        await GetEducations();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
    }
    public async Task TagDelete()
    {
            SelectedEducation.IsRemuved = 1;
        var result = await Api.PutAsync<EducationApi>(SelectedEducation, "Education");
        await GetEducations();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
    }
    async Task GetEducations()
    {
        try
        {
            var result = await Api.GetListAsync<EducationApi[]>("Education");
                Educations = new List<EducationApi>(result);
            var educations = new List<EducationApi>(Educations);
            foreach (var education in educations)
                if (education.IsRemuved == 1)
                        Educations.Remove(education);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Educations)));
            SignalChanged("Educations");


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
