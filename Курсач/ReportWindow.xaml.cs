using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace Курсач
{
    public partial class ReportWindow : Window
    {
        private ObservableCollection<Route> Routes { get; set; }
        private ObservableCollection<Repairs> Masters { get; set; }

        public ReportWindow(ObservableCollection<Route> routes, ObservableCollection<Repairs> masters)
        {
            InitializeComponent();
            Routes = routes;

            // Присваиваем источники для ComboBox
            RouteComboBox.ItemsSource = routes;

            // Проверяем, переданы ли мастера, и если да, то присваиваем источник для MasterComboBox
            if (masters != null)
            {
                Masters = masters;
                MasterComboBox.ItemsSource = masters;
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный маршрут и мастера
            Route selectedRoute = RouteComboBox.SelectedItem as Route;
            Repairs selectedMaster = MasterComboBox.SelectedItem as Repairs;

            // Получаем текст заявки на ремонт
            string repairRequestDescription = RepairRequestTextBox.Text.Trim();

            // Проверяем, что маршрут и мастер выбраны, и текст заявки не пустой
            if (selectedRoute != null && selectedMaster != null && !string.IsNullOrEmpty(repairRequestDescription))
            {
                // Создаем новую заявку на ремонт
                var repairRequest = new RepairRequest
                {
                    Description = repairRequestDescription,
                    CreationDate = DateTime.Now
                };

                // Сохраняем заявку на ремонт в базу данных
                using (var context = new AppDbContext())
                {
                    context.RepairRequests.Add(repairRequest);
                    context.SaveChanges();
                }

                // Формируем содержимое для записи в документ Word
                string content = $"Маршрут: {selectedRoute.RouteNumber}, Мастер: {selectedMaster.MasterName}, Заявка: {repairRequestDescription}";

                // Создаем приложение Word
                Word.Application wordApp = new Word.Application();
                Word.Document doc = wordApp.Documents.Add();

                // Добавляем содержимое в документ
                doc.Content.Text = content;

                // Сохраняем документ
                string fileName = $"Заявка_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.docx";
                object filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                doc.SaveAs2(filePath);

                // Закрываем и высвобождаем ресурсы Word
                doc.Close();
                wordApp.Quit();

                // Освобождаем объекты
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);

                // Оповещаем пользователя об успешном сохранении
                MessageBox.Show("Заявка успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
