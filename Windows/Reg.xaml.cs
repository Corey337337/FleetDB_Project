using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FleetDB_Project.Windows
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        public string RegisteredEmail { get; private set; }

        public Reg()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            

            string fullName = FullNameBox.Text.Trim();
            string licenceNumber = LicenceNumberBox.Text.Trim();
            string phoneNumber = PhoneNumberBox.Text.Trim();
            string experienceText = ExperienceBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string password = PasswordInput.Password;
            string repeatPassword = PasswordRepeat.Password;

            if (string.IsNullOrWhiteSpace(fullName) ||
       string.IsNullOrWhiteSpace(licenceNumber) ||
       string.IsNullOrWhiteSpace(phoneNumber) ||
       string.IsNullOrWhiteSpace(experienceText) ||
       string.IsNullOrWhiteSpace(email) ||
       string.IsNullOrEmpty(password) ||
       string.IsNullOrEmpty(repeatPassword))
            {
                MessageBox.Show("Заполните все поля.", "Проверьте данные",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Пароли не совпадают. Повторите попытку");
                return;
            }

            using (var db = new FleetDBEntities())
            {

                if (!email.Contains("@"))
                {
                    MessageBox.Show("Почта должна содержать символ @.");
                    return;
                }

                bool emailExists = db.Drivers.Any(x => x.Email == email);
                if (emailExists)
                {
                    MessageBox.Show("Данная почта уже зарегистрирована");
                    return;
                }

                var driver = new Drivers
                {
                    FullName = fullName,
                    LicenseNumber = licenceNumber,
                    PhoneNumber = phoneNumber,
                    Experience = int.Parse(experienceText),
                    Email = email,
                    Password = password
                };

                db.Drivers.Add(driver);

                try 
                {
                    db.SaveChanges();
                }
                catch(Exception)
                {

                    MessageBox.Show($"Ошибка при сохранении данных");
                    return;
                }

                RegisteredEmail = email;

                MessageBox.Show("Регистрация водителя прошла успешно!");

                DialogResult = true;

            }

        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
