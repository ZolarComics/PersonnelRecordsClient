using ModelApi;
using PersonnelRecordsClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PersonnelRecordsClient.Themes;

namespace PersonnelRecordsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow window;
        public MainWindow()
        {
            InitializeComponent();
            window = this;
            DataContext = new MainVM();
            //MainNavigate();
            List<string> styles = new List<string> { "light", "dark" };
            styleBox.SelectionChanged += ThemeChange;
            styleBox.ItemsSource = styles;
            styleBox.SelectedItem = "dark";
            //Test();
        }

        private void ThemeChange(object sender, SelectionChangedEventArgs e)
        {
            string style = styleBox.SelectedItem as string;
            // определяем путь к файлу ресурсов
            var uri = new Uri(style + ".xaml", UriKind.Relative);
            // загружаем словарь ресурсов 
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        public static void MainNavigate(Page page)
        {
            window.Frame.Navigate(page);
        }

        async Task Test()
        {
            try
            {
                var result = await Api.GetAsync<CompanyApi>(1, "Company");
            }
            catch (Exception e)
            { }
        }
    }
}
