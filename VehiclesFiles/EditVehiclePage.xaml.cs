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
    /// Логика взаимодействия для EditVehiclePage.xaml
    /// </summary>
    public partial class EditVehiclePage : Page
    {
        private readonly Vehicles vehicle;
        public EditVehiclePage(Vehicles vehicle)
        {
            InitializeComponent();
            this.vehicle = vehicle;

            using (var db = new FleetDBEntities())
            {
                var editCar = db.Vehicles.Find(vehicle.Id);

                LicensePlate.Text = editCar.LicensePlate;
                Model.Text = editCar.Model;
                Manufacturer.Text = editCar.Manufacturer;
                YearOfManafacture.Text = editCar.YearOfManafacture.ToString();
                Mileage.Text = editCar.Mileage.ToString();
                StatusBox.SelectedValue = editCar.Status;
            }
        }

        private void EditVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!int.TryParse(YearOfManafacture.Text.Trim(), out int year))
                {
                    MessageBox.Show("Год выпуска должен быть целым числом",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    YearOfManafacture.Focus();
                    return;
                }

                int maxYear = DateTime.Now.Year;
                if (year < 1900 || year > maxYear)
                {
                    MessageBox.Show($"Год выпуска должен быть от 1900 до {maxYear}",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    YearOfManafacture.Focus();
                    return;
                }

                if (!int.TryParse(Mileage.Text.Trim(), out int mileage))
                {
                    MessageBox.Show("Пробег должен быть целым числом.",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Mileage.Focus();
                    return;
                }

                if (mileage < 0)
                {
                    MessageBox.Show("Пробег должен быть больше 0",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Mileage.Focus();
                    return;
                }
                using (var db = new FleetDBEntities())
                {
                    var editVehicle = db.Vehicles.Find(vehicle.Id);

                    editVehicle.LicensePlate = LicensePlate.Text;
                    editVehicle.Model = Model.Text;
                    editVehicle.Manufacturer = Manufacturer.Text;
                    editVehicle.YearOfManafacture = Int32.Parse(YearOfManafacture.Text);
                    editVehicle.Mileage = Int32.Parse(Mileage.Text);
                    editVehicle.Status = StatusBox.SelectedValue.ToString();
                    db.SaveChanges();
                    NavigationService.Navigate(new Pages.VehiclesPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании автомобиля: {ex.Message}");
            }
        }
        

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
