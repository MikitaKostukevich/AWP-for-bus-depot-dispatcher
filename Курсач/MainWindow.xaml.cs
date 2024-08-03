using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Курсач
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Route> routes;
        private ObservableCollection<Bus> buses;
        private ObservableCollection<Driver> drivers;
        private ObservableCollection<Point> points;
        //private ObservableCollection<Route> originalRoutes;
        private ObservableCollection<Repairs> masters;
        private ObservableCollection<RepairRequest> repairRequests;

        private string userRole;


        public MainWindow()
        {
            InitializeComponent();
            LoadBusNames();

            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            // Если пользователь не авторизовался, закрываем приложение
            if (string.IsNullOrEmpty(loginWindow.UserRole))
            {
                this.Close();
                return;
            }

            // Установка роли пользователя
            userRole = loginWindow.UserRole;

            // Инициализация коллекций
            routes = new ObservableCollection<Route>();
            buses = new ObservableCollection<Bus>();
            drivers = new ObservableCollection<Driver>();
            points = new ObservableCollection<Point>();
            repairRequests = new ObservableCollection<RepairRequest>();

            // Загрузка данных из базы данных
            LoadData();

            // Привязка данных к элементам управления
            RoutesListBox.ItemsSource = routes;
            BusComboBox.ItemsSource = buses;
            DriverComboBox.ItemsSource = drivers;
            StartPointComboBox.ItemsSource = points;
            EndPointComboBox.ItemsSource = points;

            SetUserPermissions();
        }

        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                routes = new ObservableCollection<Route>(context.Routes.ToList());
                buses = new ObservableCollection<Bus>(context.Buses.ToList());
                drivers = new ObservableCollection<Driver>(context.Drivers.ToList());
                points = new ObservableCollection<Point>(context.Points.ToList());
                masters = new ObservableCollection<Repairs>(context.Repairs.ToList());
                repairRequests = new ObservableCollection<RepairRequest>(context.RepairRequests.ToList());


                // Привязка данных к элементам управления
                RoutesListBox.ItemsSource = routes;
                BusComboBox.ItemsSource = buses;
                DriverComboBox.ItemsSource = drivers;
                StartPointComboBox.ItemsSource = points;
                EndPointComboBox.ItemsSource = points;
            }
        }

        private void SetUserPermissions()
        {
            // Скрытие всех кнопок по умолчанию
            AddRouteButton.Visibility = Visibility.Collapsed;
            EditRouteButton.Visibility = Visibility.Collapsed;
            DeleteRouteButton.Visibility = Visibility.Collapsed;
            ReportRouteButton.Visibility = Visibility.Collapsed;
            RepairRequestButton.Visibility = Visibility.Collapsed;
            RepairReportButton.Visibility = Visibility.Collapsed;

            // Включение кнопок в зависимости от роли
            switch (userRole)
            {
                case "admin":
                    AddRouteButton.Visibility = Visibility.Visible;
                    EditRouteButton.Visibility = Visibility.Visible;
                    DeleteRouteButton.Visibility = Visibility.Visible;
                    ReportRouteButton.Visibility = Visibility.Visible;
                    RepairRequestButton.Visibility = Visibility.Visible;
                    RepairReportButton.Visibility = Visibility.Visible;
                    break;
                case "driver":
                    ReportRouteButton.Visibility = Visibility.Visible;
                    RepairRequestButton.Visibility = Visibility.Visible;
                    break;
                case "master":
                    RepairReportButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void AddRoute_Click(object sender, RoutedEventArgs e)
        {
            string routeNumber = RouteNumberTextBox.Text.Trim();
            Bus selectedBus = BusComboBox.SelectedItem as Bus;
            Driver selectedDriver = DriverComboBox.SelectedItem as Driver;
            Point startPoint = StartPointComboBox.SelectedItem as Point;
            Point endPoint = EndPointComboBox.SelectedItem as Point;

            if (!string.IsNullOrEmpty(routeNumber) && selectedBus != null && selectedDriver != null && startPoint != null && endPoint != null)
            {
                using (var context = new AppDbContext())
                {
                    // Загрузка связанных объектов из контекста
                    var bus = context.Buses.FirstOrDefault(b => b.Id == selectedBus.Id);
                    var driver = context.Drivers.FirstOrDefault(d => d.Id == selectedDriver.Id);
                    var start = context.Points.FirstOrDefault(p => p.PointId == startPoint.PointId);
                    var end = context.Points.FirstOrDefault(p => p.PointId == endPoint.PointId);

                    if (bus != null && driver != null && start != null && end != null)
                    {
                        // Создание и добавление нового маршрута
                        var newRoute = new Route
                        {
                            RouteNumber = routeNumber,
                            Bus = bus,
                            Driver = driver,
                            StartPoint = start.Name,
                            EndPoint = end.Name
                        };

                        context.Routes.Add(newRoute);
                        context.SaveChanges();

                        // Добавление нового маршрута в ObservableCollection
                        routes.Add(newRoute);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти связанные объекты в базе данных..", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                ClearInputs();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private void EditRoute_Click(object sender, RoutedEventArgs e)
        {
            if (RoutesListBox.SelectedItem != null)
            {
                var selectedRoute = RoutesListBox.SelectedItem as Route;
                int routeId = selectedRoute.Id;

                string routeNumber = RouteNumberTextBox.Text.Trim();
                Bus selectedBus = BusComboBox.SelectedItem as Bus;
                Driver selectedDriver = DriverComboBox.SelectedItem as Driver;
                Point startPoint = StartPointComboBox.SelectedItem as Point;
                Point endPoint = EndPointComboBox.SelectedItem as Point;

                if (!string.IsNullOrEmpty(routeNumber) && selectedBus != null && selectedDriver != null && startPoint != null && endPoint != null)
                {
                    using (var context = new AppDbContext())
                    {
                        var existingRoute = context.Routes.Find(routeId);
                        if (existingRoute != null)
                        {
                            var bus = context.Buses.FirstOrDefault(b => b.Id == selectedBus.Id);
                            var driver = context.Drivers.FirstOrDefault(d => d.Id == selectedDriver.Id);
                            var start = context.Points.FirstOrDefault(p => p.PointId == startPoint.PointId);
                            var end = context.Points.FirstOrDefault(p => p.PointId == endPoint.PointId);

                            if (bus != null && driver != null && start != null && end != null)
                            {
                                existingRoute.RouteNumber = routeNumber;
                                existingRoute.Bus = bus;
                                existingRoute.Driver = driver;
                                existingRoute.StartPoint = start.Name;
                                existingRoute.EndPoint = end.Name;

                                context.SaveChanges();

                                RoutesListBox.Items.Refresh();
                                ClearInputs();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось найти связанные объекты в базе данных..", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Маршрут не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        private void DeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            if (RoutesListBox.SelectedItem != null)
            {
                var selectedRoute = RoutesListBox.SelectedItem as Route;
                int routeId = selectedRoute.Id; // Получаем идентификатор выбранного маршрута

                // Подтверждение удаления
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот маршрут?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Находим выбранный маршрут в базе данных
                    using (var context = new AppDbContext())
                    {
                        var existingRoute = context.Routes.Find(routeId);
                        if (existingRoute != null)
                        {
                            // Удаляем маршрут из контекста базы данных
                            context.Routes.Remove(existingRoute);

                            // Сохраняем изменения в базе данных
                            context.SaveChanges();

                            // Удаляем маршрут из отображаемого списка
                            routes.Remove(selectedRoute);
                        }
                        else
                        {
                            MessageBox.Show("Маршрут не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }


        private void RoutesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoutesListBox.SelectedItem != null)
            {
                var selectedRoute = RoutesListBox.SelectedItem as Route;
                RouteNumberTextBox.Text = selectedRoute.RouteNumber;
                BusComboBox.SelectedItem = selectedRoute.Bus;
                DriverComboBox.SelectedItem = selectedRoute.Driver;
                StartPointComboBox.SelectedItem = points.FirstOrDefault(p => p.Name == selectedRoute.StartPoint);
                EndPointComboBox.SelectedItem = points.FirstOrDefault(p => p.Name == selectedRoute.EndPoint);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                var filteredRoutes = routes.Where(r => r.RouteNumber.Contains(searchText) ||
                                                       r.Bus.Name.Contains(searchText) ||
                                                       r.Driver.FullName.Contains(searchText) ||
                                                       r.StartPoint.Contains(searchText) ||
                                                       r.EndPoint.Contains(searchText));
                RoutesListBox.ItemsSource = new ObservableCollection<Route>(filteredRoutes);
            }
            else
            {
                RoutesListBox.ItemsSource = routes;
            }
        }

       

        private void ClearInputs()
        {
            RouteNumberTextBox.Clear();
            BusComboBox.SelectedIndex = -1;
            DriverComboBox.SelectedIndex = -1;
            StartPointComboBox.SelectedIndex = -1;
            EndPointComboBox.SelectedIndex = -1;
        }

        private void OpenBusWindow_Click(object sender, RoutedEventArgs e)
        {
            BusWindow busWindow = new BusWindow(buses);
            busWindow.ShowDialog();
        }

        private void GoToDriverWindow_Click(object sender, RoutedEventArgs e)
        {
            DriverWindow driverWindow = new DriverWindow(drivers);
            driverWindow.Show();
        }

        private void OpenReportWindow_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(routes, masters);
            reportWindow.ShowDialog();
        }

        private void OpenOtchetWindow_Click(object sender, RoutedEventArgs e)
        {
            OtchetWindow otchetWindow = new OtchetWindow(routes, drivers);
            otchetWindow.Show();
        }

        private void OpenPointsWindow_Click(object sender, RoutedEventArgs e)
        {
            PointsWindow pointsWindow = new PointsWindow(points);
            pointsWindow.Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            LoadBusNames();
        }
        private void OpenMasterReportWindow_Click(object sender, RoutedEventArgs e)
        {
            MasterReportWindow masterReportWindow = new MasterReportWindow(masters, repairRequests);
            masterReportWindow.ShowDialog();
        }

        private void ManageMastersButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новое окно для управления мастерами и отображаем его
            ManageMastersWindow manageMastersWindow = new ManageMastersWindow(DatabaseHelper.GetRepairs());
            manageMastersWindow.ShowDialog();
        }

        private void SortRoutes_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранные значения из ComboBox
            string sortBy = (SortByComboBox.SelectedItem as ComboBoxItem)?.Content as string;
            string sortDirection = (SortDirectionComboBox.SelectedItem as ComboBoxItem)?.Content as string;

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
            {
                switch (sortBy)
                {
                    case "Номер маршрута":
                        if (sortDirection == "Возрастание")
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderBy(r => r.RouteNumber));
                        else
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderByDescending(r => r.RouteNumber));
                        break;
                    case "Автобус":
                        if (sortDirection == "Возрастание")
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderBy(r => r.Bus.Name));
                        else
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderByDescending(r => r.Bus.Name));
                        break;
                    case "Водитель":
                        if (sortDirection == "Возрастание")
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderBy(r => r.Driver.FullName));
                        else
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderByDescending(r => r.Driver.FullName));
                        break;
                    case "Начальная точка":
                        if (sortDirection == "Возрастание")
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderBy(r => r.StartPoint));
                        else
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderByDescending(r => r.StartPoint));
                        break;
                    case "Конечная точка":
                        if (sortDirection == "Возрастание")
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderBy(r => r.EndPoint));
                        else
                            RoutesListBox.ItemsSource = new ObservableCollection<Route>(routes.OrderByDescending(r => r.EndPoint));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите оба критерия сортировки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBusNames()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Получаем список уникальных названий автобусов из БД
                    var busNames = context.Buses.Select(b => b.Name).Distinct().ToList();

                    // Заполняем ComboBox списком названий автобусов
                    BusNameFilterComboBox.ItemsSource = busNames;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке названий автобусов из БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FilterRoutesByBusName(string busName)
        {
            if (!string.IsNullOrEmpty(busName))
            {
                var filteredRoutes = routes.Where(r => r.Bus.Name == busName);
                RoutesListBox.ItemsSource = new ObservableCollection<Route>(filteredRoutes);
            }
        }

        private void FilterRoutesByBusNameButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedBusName = BusNameFilterComboBox.SelectedItem as string;
            FilterRoutesByBusName(selectedBusName);
        }
        private void ShowInstructions_Click(object sender, RoutedEventArgs e)
        {
            string instructions = "Добро пожаловать в программу управления маршрутами!\n\n" +
                                  "Для добавления нового маршрута заполните поля в верхней части окна " +
                                  "и нажмите кнопку 'Добавить маршрут'.\n" +
                                  "Вы также можете изменить или удалить существующий маршрут, выбрав его из списка и " +
                                  "нажав соответствующую кнопку.\n" +
                                  "Используйте фильтры и сортировку, чтобы настроить отображение маршрутов по вашим предпочтениям.\n" +
                                  "Для добавления водителя, автобуса, точки маршрута нажмите на соответствующие кнопки. ";

            MessageBox.Show(instructions, "Инструкция", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }


}
