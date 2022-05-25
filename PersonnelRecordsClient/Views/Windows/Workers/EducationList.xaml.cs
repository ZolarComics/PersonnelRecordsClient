﻿using PersonnelRecordsClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonnelRecordsClient.Views.Windows.Workers
{
    /// <summary>
    /// Логика взаимодействия для EducationList.xaml
    /// </summary>
    public partial class EducationList : Window
    {
        public EducationList()
        {
            InitializeComponent();
            DataContext = new EducationPageVM();
        }
    }
}
