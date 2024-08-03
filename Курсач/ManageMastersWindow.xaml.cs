using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Курсач
{
    public partial class ManageMastersWindow : Window
    {
        private ObservableCollection<Repairs> masters;

        public ManageMastersWindow(ObservableCollection<Repairs> masters)
        {
            InitializeComponent();
            this.masters = masters;
            MastersDataGrid.ItemsSource = masters;
        }

        private void AddMasterButton_Click(object sender, RoutedEventArgs e)
        {
            string masterName = MasterNameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(masterName))
            {
                var newMaster = new Repairs(masterName);
                DatabaseHelper.AddRepair(newMaster);
                masters.Add(newMaster);
                MasterNameTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите имя мастера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateMasterButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaster = MastersDataGrid.SelectedItem as Repairs;
            if (selectedMaster != null)
            {
                string newMasterName = MasterNameTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(newMasterName))
                {
                    selectedMaster.MasterName = newMasterName;
                    DatabaseHelper.UpdateRepair(selectedMaster);
                    MastersDataGrid.Items.Refresh();
                    MasterNameTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите новое имя мастера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите мастера для изменения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteMasterButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaster = MastersDataGrid.SelectedItem as Repairs;
            if (selectedMaster != null)
            {
                DatabaseHelper.DeleteRepair(selectedMaster.Id);
                masters.Remove(selectedMaster);
                MasterNameTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите мастера для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MastersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedMaster = MastersDataGrid.SelectedItem as Repairs;
            if (selectedMaster != null)
            {
                MasterNameTextBox.Text = selectedMaster.MasterName;
            }
        }
    }
}
