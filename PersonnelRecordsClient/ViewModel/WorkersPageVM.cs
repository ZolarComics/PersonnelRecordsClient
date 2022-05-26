using ModelApi;
using PersonnelRecordsClient.MVVM;
using PersonnelRecordsClient.ViewModel.WindowsVM;
using PersonnelRecordsClient.Views.Windows.Companies.Staffing;
using PersonnelRecordsClient.Views.Windows.Workers;
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
   internal class WorkersPageVM : BaseViewModel, INotifyPropertyChanged
    {            
        public event PropertyChangedEventHandler PropertyChanged;
       
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
        // 2 свойства - в одном выбранная запись из коллекции, во втором - копия выбранной записи. По сохранению - первое свойство через post в архив, второе через put обратно в контроллер воркера
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
        
        public List<WorkerApi> Workers { get; set; }
        public List<WorkerApi> Archives { get; set; }
        public List<ExperienceApi> ExperienceList { get; set; }
        public List<CompanyApi> Companies { get; set; }

        public CustomCommand GoEditWorker { get; set; }
        public CustomCommand AddWorker { get; set; }
        public CustomCommand EditWorker { get; set; }
        public CustomCommand SaveWorker { get; set; }
        public CustomCommand RemoveWorker { get; set; }
        public CustomCommand GoEdit { get; set; }

        public CustomCommand RemoveManyWorker { get; set; }
              
        public WorkersPageVM()
        {
            GoEditWorker = new CustomCommand(() =>
            {
                EditWorker EditWorker = new EditWorker();
                EditWorker.Show();
            });

            RemoveManyWorker = new CustomCommand(() =>
            {
                Task.Run(Delete);
            });

            AddWorker = new CustomCommand(() =>
            {
                Task.Run(Add);
            });

            SaveWorker = new CustomCommand(() =>
            {
                Task.Run(AddArchive);
                Task.Run(Save);               
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            RemoveWorker = new CustomCommand(() =>
            {                
                Task.Run(TagDelete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            GoEdit = new CustomCommand(() =>
            {
                //MainWindow.MainNavigate(new EditWorker());
            });
            Task.Run(GetExperienceMethod);
            //Task.Run(GetArchive);
            Task.Run(GetExperienceMethod);
            Task.Run(Delete);
            Task.Run(GetWorkers);
        }
        public CustomCommand GetExperience { get; set; }

        List<ExperienceApi> ExperiencePosition { get; set; }

        public TimeSpan ExperienceTimeSpan { get; set; }


        public async Task GetExperienceWorker(WorkerExpGetDates options)
        {
            var resultArchive = await Api.PostAsync<ArchiveApi>(SelectedArchive, "Archive");
            var resultExp = await Api.PostGetAsync<WorkerExpGetDates, WorkerExp>(options, "/ArchiveGet");
            var experiencePosition = await Api.GetListAsync<List<ExperienceApi>>("Experience");
           // int Exp = SelectedArchive.DateRecord.In;

            TimeSpan time = new TimeSpan();
            foreach (var f in resultExp.History)
            {
                if (!f.End.HasValue == true)
                {
                    ExperienceTimeSpan = time.Add(f.End.Value.Subtract(f.Start.Value));
                }
                else if (!f.End.HasValue == false)
                {
                    f.End = DateTime.Now;
                    ExperienceTimeSpan = time.Add(f.End.Value.Subtract(f.Start.Value));
                }
            }

            DateTime date1 = new DateTime(1996, 6, 3, 22, 15, 0);
            DateTime date2 = new DateTime(1996, 12, 6, 13, 2, 0);
            DateTime date3 = new DateTime(1996, 10, 12, 8, 42, 0);

            // diff1 gets 185 days, 14 hours, and 47 minutes.
            TimeSpan diff1 = date2.Subtract(date1);

            // date4 gets 4/9/1996 5:55:00 PM.
            DateTime date4 = date3.Subtract(diff1);

            // diff2 gets 55 days 4 hours and 20 minutes.
            TimeSpan diff2 = date2 - date3;

            // date5 gets 4/9/1996 5:55:00 PM.
            DateTime date5 = date1 - diff2;



        }



      /*  public async Task AddArchive()
        {
              DateTime TodayTime = DateTime.Now();
             SelectedArchive = new ArchiveApi { OneRecord = SelectedWorker.Name, DateRecord = TodayTime };
            var result = Api.PostAsync(SelectedWorker, "Worker");
        }
      */

        public async Task TagDelete()
        {
            SelectedWorker.IsRemuved = 1;
            var result = await Api.PutAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }

        public async Task Add()
        {
            SelectedWorker = new WorkerApi();
           // SelectedWorker = new List<WorkerApi>();
            var result = Api.PostAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }


        DateTime TodayTime = DateTime.Now;

        public async Task AddArchive()
        {
            SelectedArchive = new ArchiveApi 
            { OneRecord = SelectedWorker.Name, 
                TwoRecord = SelectedWorker.Surname, 
                ThreeRecord = SelectedWorker.Patronymic, 
                FourRecord = SelectedWorker.Phone,
                DateRecord = TodayTime
            };
            var result = Api.PostAsync(SelectedArchive, "Archive");
           
        }

        public async Task Save()
        {           
            var result = await Api.PutAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }

        public async Task Delete()
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

        async Task GetCompanies()
        {
            try
            {
                var resultCompanies = await Api.GetListAsync<CompanyApi[]>("Company");
                Companies = new List<CompanyApi>(resultCompanies);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companies)));
                SignalChanged("Companies");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }

        async Task GetExperienceMethod()
        {
            try
            {
                var result = await Api.GetListAsync<ExperienceApi[]>("Experience");
                ExperienceList = new List<ExperienceApi>(result);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExperienceList)));
                SignalChanged("Experience");
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