using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Курсач
{
    public partial class DriverWindow : Window
    {
        private ObservableCollection<Driver> drivers;

        public DriverWindow(ObservableCollection<Driver> drivers)
        {
            InitializeComponent();
            this.drivers = drivers;
            DriversDataGrid.ItemsSource = this.drivers;
        }

        private void AddDriverButton_Click(object sender, RoutedEventArgs e)
        {
            var newDriver = new Driver
            {
                FullName = FullNameTextBox.Text,
                Age = int.Parse(AgeTextBox.Text),
                Gender = GenderTextBox.Text,
                Address = AddressTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                DrivingExperience = int.Parse(DrivingExperienceTextBox.Text)
            };

            using (var context = new AppDbContext())
            {
                context.Drivers.Add(newDriver);
                context.SaveChanges();
            }

            drivers.Add(newDriver);
        }

        private void UpdateDriverButton_Click(object sender, RoutedEventArgs e)
        {
            if (DriversDataGrid.SelectedItem is Driver selectedDriver)
            {
                selectedDriver.FullName = FullNameTextBox.Text;
                selectedDriver.Age = int.Parse(AgeTextBox.Text);
                selectedDriver.Gender = GenderTextBox.Text;
                selectedDriver.Address = AddressTextBox.Text;
                selectedDriver.PhoneNumber = PhoneNumberTextBox.Text;
                selectedDriver.DrivingExperience = int.Parse(DrivingExperienceTextBox.Text);

                using (var context = new AppDbContext())
                {
                    context.Entry(selectedDriver).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                DriversDataGrid.Items.Refresh();
            }
        }

        private void DeleteDriverButton_Click(object sender, RoutedEventArgs e)
        {
            if (DriversDataGrid.SelectedItem is Driver selectedDriver)
            {
                using (var context = new AppDbContext())
                {
                    // Перезагрузить объект из контекста данных, чтобы убедиться, что он находится в состоянии отслеживания
                    var driverToDelete = context.Drivers.FirstOrDefault(d => d.Id == selectedDriver.Id);
                    if (driverToDelete != null)
                    {
                        context.Drivers.Remove(driverToDelete);
                        context.SaveChanges();

                        // Обновить коллекцию drivers после удаления
                        drivers.Remove(selectedDriver);
                    }
                }
            }
        }



        private void DriversDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DriversDataGrid.SelectedItem is Driver selectedDriver)
            {
                FullNameTextBox.Text = selectedDriver.FullName;
                AgeTextBox.Text = selectedDriver.Age.ToString();
                GenderTextBox.Text = selectedDriver.Gender;
                AddressTextBox.Text = selectedDriver.Address;
                PhoneNumberTextBox.Text = selectedDriver.PhoneNumber;
                DrivingExperienceTextBox.Text = selectedDriver.DrivingExperience.ToString();
            }
        }
    }
}
