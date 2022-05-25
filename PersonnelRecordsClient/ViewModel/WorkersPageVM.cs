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
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            GoEdit = new CustomCommand(() =>
            {
                //MainWindow.MainNavigate(new EditWorker());
            });
            Task.Run(GetWorkers);
        }
        public CustomCommand GetExperience { get; set; }

        List<ExperienceApi> ExperiencePosition { get; set; }

        public TimeSpan Experience { get; set; }
        public async Task GetExperienceWorker(WorkerExpGetDates options)
        {
            //var resultWorker = await Api.PostAsync<WorkerApi>(SelectedWorker[0], "Worker");
            var resultExp = await Api.PostGetAsync<WorkerExpGetDates, WorkerExp>(options, "/ArchiveGet");
            var experiencePosition = await Api.GetListAsync<List<ExperienceApi>>("Experience");
            ExperiencePosition = experiencePosition;

            TimeSpan time = new TimeSpan();
            foreach (var f in resultExp.History)
            {
                if (!f.End.HasValue == true)
                {
                    Experience = time.Add(f.End.Value.Subtract(f.Start.Value));
                }
                else if (!f.End.HasValue == false)
                {
                    f.End = DateTime.Now;
                    Experience = time.Add(f.End.Value.Subtract(f.Start.Value));
                }
            }
        }
        public async Task Add()
        {
            SelectedWorker = new WorkerApi();
           // SelectedWorker = new List<WorkerApi>();
            var result = Api.PostAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }        
        public async Task AddArchive()
        {                     
            SelectedArchive = new ArchiveApi { OldRecord = SelectedWorker.Name, NewRecord = SelectedWorker.Surname };
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
       
        void SignalChanged([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}