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
        private  Dispatcher dispatcher;       
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

        // private List<WorkerApi> workers;
        public List<WorkerApi> Workers { get; set; }
        public List<WorkerApi> Archives { get; set; }
        public CustomCommand GoEditWorker { get; set; }
        public CustomCommand AddWorker { get; set; }
        public CustomCommand EditWorker { get; set; }
        public CustomCommand SaveWorker { get; set; }
        public CustomCommand RemoveWorker { get; set; }
        public CustomCommand GoStaffing { get; set; }


        public WorkersPageVM()
        {
            GoEditWorker = new CustomCommand(() =>
            {
                EditWorker EditWorker = new EditWorker();
                EditWorker.Show();
                //EditWorker = new EditWorker.Show();
                //MainWindow.MainNavigate(new EditWorker());
            });
        }


        //public WorkersPageVM(List<WorkerApi> Workers)
        //{
            
            
        //}
        public WorkersPageVM(Dispatcher dispatcher)
        {
            //string Old = SelectedWorker.Surname;
            AddWorker = new CustomCommand(() =>
            {
                Task.Run(Add);
            });
            SaveWorker = new CustomCommand(() =>
            { 
                string x = SelectedWorker.Surname;
                Task.Run(Save);
                string y = SelectedWorker.Surname;
                //Task.Run(AddArchive(y, x));
                Task.Run(AddArchive);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            RemoveWorker = new CustomCommand(() =>
            {
                Task.Run(Delete);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
            });
            GoStaffing = new CustomCommand(() =>
            {
                MainWindow.MainNavigate(new AppointWorker());
            });
            Task.Run(GetWorkers);
        }
        public CustomCommand GetExperience { get; set; }

        List<ExperienceApi> ExperiencePosition { get; set; }

        public TimeSpan Experience { get; set; }
        public async Task GetExperienceWorker(WorkerExpGetDates options)
        {
            var resultWorker = await Api.PostAsync<WorkerApi>(SelectedWorker, "Worker");
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
            var result = Api.PostAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }
        //public async Task AddArchive(string y, string x)
        public async Task AddArchive()
        {
            string OldSurname = SelectedWorker.Surname;
            string NewSurname = SelectedWorker.Surname;
            //string OldSurname = SelectedWorker.Surname;
            //string Name = SelectedWorker.Name;
            //string Phone = SelectedWorker.Phone;
            //string Patronymic = SelectedWorker.Patronymic;
            //string Email = SelectedWorker.Email;
            //string Patronymic = SelectedWorker.Patronymic;
            var archive = new ArchiveApi { OldRecord = OldSurname, NewRecord = NewSurname };
            var result = Api.PostAsync<ArchiveApi>(SelectedArchive, "archive");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Archives)));
        }

        public async Task Save()
        {
            string OldSurname = SelectedWorker.Surname;
            var oldWorker = SelectedWorker;
            var result = await Api.PutAsync<WorkerApi>(SelectedWorker, "Worker");
            await GetWorkers();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
        }

        public async  Task Delete()
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

        public async Task LoadEntities()
        {
            var result = await Api.GetListAsync<WorkerApi>("Worker");
        }
        void SignalChanged([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}