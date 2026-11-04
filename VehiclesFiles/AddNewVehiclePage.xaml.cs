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

namespace FleetDB_Project.VehiclesFiles
{
    /// <summary>
    /// Логика взаимодействия для AddNewVehiclePage.xaml
    /// </summary>
    public partial class AddNewVehiclePage : Page
    {
        public AddNewVehiclePage()
        {
            InitializeComponent();
        }

        private void SaveVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FleetDBEntities())
                {
                    Vehicles v = new Vehicles();
                    db.Vehicles.Add(v);
                    v.LicensePlate = LicensePlate.Text;
                    v.Model = Model.Text;
                    v.Manufacturer = Manufacturer.Text;
                    v.Mileage = int.Parse(Mileage.Text);
                    v.YearOfManafacture = int.Parse(YearOfManafacture.Text);
                    v.Status = StatusBox.SelectedValue.ToString();

                    db.SaveChanges();
                    NavigationService.Navigate(new Pages.VehiclesPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить автомобиль! Проверьте подключение к БД! {ex.Message}",
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
