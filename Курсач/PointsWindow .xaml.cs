using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Курсач
{
    public partial class PointsWindow : Window
    {
        private ObservableCollection<Point> points;

        public PointsWindow(ObservableCollection<Point> points)
        {
            InitializeComponent();
            this.points = points;
            PointsDataGrid.ItemsSource = this.points;
        }

        private void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            var newPoint = new Point
            {
                Name = NameTextBox.Text,
                Address = AddressTextBox.Text
            };

            using (var context = new AppDbContext())
            {
                context.Points.Add(newPoint);
                context.SaveChanges();
            }

            points.Add(newPoint);
        }

        private void UpdatePointButton_Click(object sender, RoutedEventArgs e)
        {
            if (PointsDataGrid.SelectedItem is Point selectedPoint)
            {
                selectedPoint.Name = NameTextBox.Text;
                selectedPoint.Address = AddressTextBox.Text;

                using (var context = new AppDbContext())
                {
                    context.Entry(selectedPoint).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                PointsDataGrid.Items.Refresh();
            }
        }

        private void DeletePointButton_Click(object sender, RoutedEventArgs e)
        {
            if (PointsDataGrid.SelectedItem is Point selectedPoint)
            {
                using (var context = new AppDbContext())
                {
                    // Перезагрузите объект из контекста данных, чтобы убедиться, что он отслеживается
                    var pointToDelete = context.Points.FirstOrDefault(p => p.PointId == selectedPoint.PointId);
                    if (pointToDelete != null)
                    {
                        context.Points.Remove(pointToDelete);
                        context.SaveChanges();

                        // Обновите коллекцию points после удаления
                        points.Remove(selectedPoint);
                    }
                }
            }
        }


        private void PointsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PointsDataGrid.SelectedItem is Point selectedPoint)
            {
                NameTextBox.Text = selectedPoint.Name;
                AddressTextBox.Text = selectedPoint.Address;
            }
        }
    }
}
