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

namespace FleetDB_Project.RoutesFiles
{
    /// <summary>
    /// Логика взаимодействия для AddNewRoutePage.xaml
    /// </summary>
    public partial class AddNewRoutePage : Page
    {
        public AddNewRoutePage()
        {
            InitializeComponent();
            using (var db = new FleetDBEntities())
            {
                DriverComboBox.ItemsSource = db.Drivers.ToList();
                DriverComboBox.SelectedIndex = 0;
                VehicleComboBox.ItemsSource = db.Vehicles.ToList();
                VehicleComboBox.SelectedIndex = 0;
                StartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void SaveVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FleetDBEntities())
                {

                    int vehicleId = Int32.Parse(VehicleComboBox.SelectedValue.ToString());
                    var vehicle = db.Vehicles.Find(vehicleId);

                    if (vehicle.Status != "Доступен")
                    {
                        MessageBox.Show("Эта машина сейчас недоступна");
                        return;
                    }

                    Routes route = new Routes();
                    db.Routes.Add(route);
                    route.DriverId = Int32.Parse(DriverComboBox.SelectedValue.ToString());
                    route.VehicleId = vehicleId;
                    route.StartDate = DateTime.Parse(StartDate.Text);
                    route.StartLocation = StartLocation.Text;
                    route.EndLocation = EndLocation.Text;
                    route.Distance = int.Parse(Distance.Text);

                    vehicle.Status = "В рейсе";

                    db.SaveChanges();
                    NavigationService.Navigate(new Pages.RoutesPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить маршрут! Проверьте корректность введенных данных и подключение к БД! {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
