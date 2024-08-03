using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace Курсач
{
    public partial class MasterReportWindow : System.Windows.Window
    {
        private ObservableCollection<Repairs> masters;
        private ObservableCollection<RepairRequest> repairRequests;

        public MasterReportWindow(ObservableCollection<Repairs> masters, ObservableCollection<RepairRequest> repairRequests)
        {
            InitializeComponent();

            this.masters = masters;
            this.repairRequests = repairRequests;

            MasterComboBox.ItemsSource = masters;
            RepairRequestComboBox.ItemsSource = repairRequests;
        }

        private void SaveReportButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaster = MasterComboBox.SelectedItem as Repairs;
            var selectedRepairRequest = RepairRequestComboBox.SelectedItem as RepairRequest;
            string reportText = ReportTextBox.Text.Trim();

            if (selectedMaster != null && selectedRepairRequest != null && !string.IsNullOrEmpty(reportText))
            {
                try
                {
                    Word.Application wordApp = new Word.Application();
                    Word.Document document = wordApp.Documents.Add();

                    var paragraph = document.Paragraphs.Add();
                    paragraph.Range.Text = $"Мастер: {selectedMaster.MasterName}\nЗаявка: {selectedRepairRequest.Description}\nОтчет:\n{reportText}";

                    string fileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Ремонт_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                    document.SaveAs2(fileName);
                    document.Close();
                    wordApp.Quit();

                    MessageBox.Show($"Отчет успешно сохранен в файл {fileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
