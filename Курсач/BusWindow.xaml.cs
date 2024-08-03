using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Курсач
{
    public partial class BusWindow : Window
    {
        private ObservableCollection<Bus> buses;

        public BusWindow(ObservableCollection<Bus> buses)
        {
            InitializeComponent();
            this.buses = buses;
            BusesDataGrid.ItemsSource = this.buses;
        }

        private void AddBusButton_Click(object sender, RoutedEventArgs e)
        {
            var newBus = new Bus
            {
                Name = NameTextBox.Text,
                Model = ModelTextBox.Text,
                Year = int.Parse(YearTextBox.Text),
                Seats = int.Parse(SeatsTextBox.Text)
            };

            using (var context = new AppDbContext())
            {
                context.Buses.Add(newBus);
                context.SaveChanges();
            }

            buses.Add(newBus);
        }

        private void UpdateBusButton_Click(object sender, RoutedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem is Bus selectedBus)
            {
                selectedBus.Name = NameTextBox.Text;
                selectedBus.Model = ModelTextBox.Text;
                selectedBus.Year = int.Parse(YearTextBox.Text);
                selectedBus.Seats = int.Parse(SeatsTextBox.Text);

                using (var context = new AppDbContext())
                {
                    context.Entry(selectedBus).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                BusesDataGrid.Items.Refresh();
            }
        }

        private void DeleteBusButton_Click(object sender, RoutedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem is Bus selectedBus)
            {
                using (var context = new AppDbContext())
                {
                    var existingBus = context.Buses.FirstOrDefault(b => b.Id == selectedBus.Id);
                    if (existingBus != null)
                    {
                        context.Buses.Remove(existingBus);
                        context.SaveChanges();
                        buses.Remove(selectedBus);
                    }
                }

            }
        }

        private void BusesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem is Bus selectedBus)
            {
                NameTextBox.Text = selectedBus.Name;
                ModelTextBox.Text = selectedBus.Model;
                YearTextBox.Text = selectedBus.Year.ToString();
                SeatsTextBox.Text = selectedBus.Seats.ToString();
            }
        }
    }
}
