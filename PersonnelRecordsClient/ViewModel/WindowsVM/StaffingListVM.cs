using ModelApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PersonnelRecordsClient.ViewModel.WindowsVM
{
    class StaffingListVM : INotifyPropertyChanged
    {
        public StaffingApi SelectedStaffing { get; set; }
        public List<StaffingApi> Staffings { get; set; }

        public StaffingListVM()
        {
          //Task.Run(GetStaffings);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //async Task GetStaffings()
        //{
        //    try
        //    {
        //        var result = await Api.GetListAsync<StaffingApi[]>("Staffing");
        //        Staffings = new List<StaffingApi>(result);
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Staffings)));

        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show($"{e}");
        //    }
        //}
    }
}
