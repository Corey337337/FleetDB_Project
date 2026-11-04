using FleetDB_Project.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetDB_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenRegWin_Click(object sender, RoutedEventArgs e)
        {
            Reg regWindow = new Reg();
            regWindow.Owner = this;

            if (regWindow.ShowDialog() == true)
            {
                EmailBox.Text = regWindow.RegisteredEmail;
                PasswordInput.Clear();
                PasswordInput.Focus();
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordInput.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите почту и пароль.");
                return;
            }

            using (var db = new FleetDBEntities())
            {
                var user = db.Drivers.FirstOrDefault(u => u.Email == email);

                if (user == null || password!=user.Password)
                {
                    MessageBox.Show("Неверная почта или пароль.");
                    return;
                }

            }


            var homeWin = new Home();
            Application.Current.MainWindow = homeWin;
            homeWin.Show();
            Close();
        }
    }
}
