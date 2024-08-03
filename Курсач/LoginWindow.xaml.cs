using System.Linq;
using System.Windows;

namespace Курсач
{
    public partial class LoginWindow : Window
    {
        public string UserRole { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            UserRole = string.Empty;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (var context = new AppDbContext())
            {
                // Проверка логина и пароля в базе данных
                var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    UserRole = user.Role;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }
    }
}
