using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Курсач
{
    public partial class OtchetWindow : Window
    {
        private ObservableCollection<Route> routes;
        private ObservableCollection<Driver> drivers;

        public OtchetWindow(ObservableCollection<Route> routes, ObservableCollection<Driver> drivers)
        {
            InitializeComponent();

            // Присваиваем списки маршрутов и водителей из главного окна
            this.routes = routes;
            this.drivers = drivers;

            // Привязываем списки к ComboBox
            RouteComboBox.ItemsSource = this.routes;
            DriverComboBox.ItemsSource = this.drivers;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            Route selectedRoute = RouteComboBox.SelectedItem as Route;
            Driver selectedDriver = DriverComboBox.SelectedItem as Driver;
            string reportText = ReportTextBox.Text.Trim();

            if (selectedRoute != null && selectedDriver != null && !string.IsNullOrEmpty(reportText))
            {
                string reportInfo = $"Маршрут: {selectedRoute.RouteNumber}, Водитель: {selectedDriver.FullName}, Отчёт: {reportText}";

                string fileName = $"Отчёт_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.doc";

                try
                {
                    using (StreamWriter writer = new StreamWriter(fileName, true))
                    {
                        writer.WriteLine(reportInfo);
                    }

                    MessageBox.Show($"Отчёт успешно сохранен. {fileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчёта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите маршрут, водителя и заполните отчёт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
