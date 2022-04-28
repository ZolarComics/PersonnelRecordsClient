using ModelApi;
using PersonnelRecordsClient.MVVM;
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
        public CustomCommand CreateWorker { get; set; }
        public CustomCommand EditWorker { get; set; }
        public CustomCommand RemoveWorker { get; set; }
        public CustomCommand TestCommand { get; set; }




        public WorkersPageVM(List<WorkerApi> Workers)
        {
            TestCommand = new CustomCommand(() =>
            {
                MessageBoxResult result = MessageBox.Show("Тест", "Комманда работает",
                MessageBoxButton.YesNo);
            }
                );
            CreateWorker = new CustomCommand(() => {
                EditWorker editWorker = new EditWorker();
                editWorker.ShowDialog();
            });

            EditWorker = new CustomCommand(() => {
                if (SelectedWorker == null)
                    return;
                EditWorker editWorker = new EditWorker();
                editWorker.ShowDialog();
            });
            RemoveWorker = new CustomCommand(() =>
            {

                Task.Run(DeleteWorkers);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Workers)));
                //if (SelectedWorker == null)
                //{
                //    System.Windows.MessageBox.Show("Для удаления клиента нужно его выбрать в списке");
                //    return;
                //}
                //MessageBoxResult result = MessageBox.Show("Удалить?", "Да?",
                //MessageBoxButton.YesNo,
                //MessageBoxImage.Question);

                //if (result == MessageBoxResult.Yes)
                //{
                //    try
                //    {
                //        foreach (var worker in Workers)
                //        {
                //            DeleteWorkers(worker);
                //            SignalChanged("Workers");
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        MessageBox.Show(e.Message);
                //    }
                //}
            });
        }
        public WorkersPageVM(Dispatcher dispatcher)
        {
            Task.Run(GetWorkers);
        }

       public async  Task DeleteWorkers()
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