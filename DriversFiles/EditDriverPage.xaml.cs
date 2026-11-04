using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace FleetDB_Project.DriversFiles
{
    /// <summary>
    /// Логика взаимодействия для EditDriverPage.xaml
    /// </summary>
    public partial class EditDriverPage : Page
    {
        private readonly Drivers driver;
        public EditDriverPage(Drivers driver)
        {
            InitializeComponent();

            this.driver = driver;

            using (var db = new FleetDBEntities())
            {
                var editCar = db.Drivers.Find(driver.Id);

                FullName.Text = editCar.FullName;
                LicenseNumber.Text = editCar.LicenseNumber;
                PhoneNumber.Text = editCar.PhoneNumber;
                Experience.Text = editCar.Experience.ToString();
            }

        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FleetDBEntities())
                {
                    var editDriver = db.Drivers.Find(driver.Id);

                    editDriver.FullName = FullName.Text;
                    editDriver.LicenseNumber = LicenseNumber.Text;
                    editDriver.PhoneNumber = PhoneNumber.Text;
                    editDriver.Experience = Int32.Parse(Experience.Text);
                    db.SaveChanges();
                    NavigationService.Navigate(new Pages.DriversPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании водителя: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
