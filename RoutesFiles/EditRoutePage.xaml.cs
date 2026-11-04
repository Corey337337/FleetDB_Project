using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для EditRoutePage.xaml
    /// </summary>
    public partial class EditRoutePage : Page
    {

        private readonly Routes route;

        public EditRoutePage(Routes route)
        {
            InitializeComponent();
            this.route = route;

            using (var db = new FleetDBEntities())
            {

                DriverComboBox.ItemsSource = db.Drivers.ToList();
                DriverComboBox.SelectedIndex = 0;
                VehicleComboBox.ItemsSource = db.Vehicles.ToList();
                VehicleComboBox.SelectedIndex = 0;

                var editRoute = db.Routes.Find(route.Id);

                DriverComboBox.SelectedValue = editRoute.DriverId;
                VehicleComboBox.SelectedValue = editRoute.VehicleId; 
                StartDate.Text = editRoute.StartDate.ToString();
                EndDate.Text = editRoute.EndDate.ToString();
                StartLocation.Text = editRoute.StartLocation;
                EndLocation.Text = editRoute.EndLocation;
                Distance.Text = editRoute.Distance.ToString();

            }
        }

        private void EditRoute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FleetDBEntities())
                {
                    var editRoute = db.Routes.Find(route.Id);

                    editRoute.DriverId = (int)DriverComboBox.SelectedValue;
                    editRoute.VehicleId = (int)VehicleComboBox.SelectedValue;
                    editRoute.StartDate = DateTime.Parse(StartDate.Text);
                    editRoute.EndDate = DateTime.Parse(EndDate.Text);
                    editRoute.StartLocation = StartLocation.Text;
                    editRoute.EndLocation = EndLocation.Text;
                    editRoute.Distance = int.Parse(Distance.Text);
                    db.SaveChanges();
                    NavigationService.Navigate(new Pages.RoutesPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании маршрута: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

        }
    }
}
