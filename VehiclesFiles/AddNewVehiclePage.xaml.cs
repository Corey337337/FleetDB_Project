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
                if (!int.TryParse(YearOfManafacture.Text.Trim(), out int year))
                {
                    MessageBox.Show("Год выпуска должен быть целым числом",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    YearOfManafacture.Focus();
                    return;
                }

                int maxYear = DateTime.Now.Year + 1;
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
                MessageBox.Show($"Не удалось сохранить автомобиль! {ex.Message}",
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
