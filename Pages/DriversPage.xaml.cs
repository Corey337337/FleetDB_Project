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
using FleetDB_Project.DriversFiles;

namespace FleetDB_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для DriversPage.xaml
    /// </summary>
    public partial class DriversPage : Page
    {
        public DriversPage()
        {
            InitializeComponent();
            LoadExperience();
            LoadDrivers();
            
        }

        private void LoadExperience()
        {
            using (var db = new FleetDBEntities())
            {
                var experiences = db.Drivers.Select(x => x.Experience).
                    Distinct().OrderBy(x => x).ToList().Select(x => x.ToString())
            .ToList();
                experiences.Insert(0, "Все");
                ExpBox.ItemsSource = experiences;
                ExpBox.SelectedIndex = 0;
            }
        }

        public void LoadDrivers()
        {
            try
            {
                string search = SearchBox.Text.Trim();

                using (var db = new FleetDBEntities())
                {
                    var drivers = db.Drivers.AsQueryable();

                    if (!string.IsNullOrEmpty(search))
                    {
                        drivers = drivers.Where(x => x.FullName.Contains(search));
                    }

                    if (ExpBox.SelectedIndex > 0)
                    {
                        int expValue = int.Parse(ExpBox.SelectedItem.ToString());
                        drivers = drivers.Where(x => x.Experience == expValue);
                    }

                    switch (SortBox.SelectedIndex)
                    {
                        case 0:
                            drivers = drivers.OrderBy(x => x.FullName).ThenBy(c => c.Experience);
                            break;
                        case 1:
                            drivers = drivers.OrderByDescending(x => x.FullName).ThenBy(c => c.Experience);
                            break;
                        case 2:
                            drivers = drivers.OrderBy(x => x.Experience);
                            break;
                        case 3:
                            drivers = drivers.OrderByDescending(x => x.Experience);
                            break;
                    }

                    DriversGrid.ItemsSource = drivers.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось получить водителей! Проверьте подключение к БД! {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }

        }



        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            LoadDrivers();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            SortBox.SelectedIndex = 0;
            ExpBox.SelectedIndex = 0;
            LoadDrivers();
        }

        private void AddNewDriver_Click(object sender, RoutedEventArgs e)
        {
            var reg = new FleetDB_Project.Windows.Reg();
            if (reg.ShowDialog() == true)
            {
                LoadDrivers();
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var driver = button?.DataContext as Drivers;

            NavigationService.Navigate(new DriversFiles.EditDriverPage(driver));

        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var driver = button?.DataContext as Drivers;

            if (driver == null)
            {
                return;
            }
            var result = MessageBox.Show($"Вы уверены что хотите удалить?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            using (var db = new FleetDBEntities())
            {
                if (db.Routes.Any(r => r.DriverId == driver.Id))
                {
                    MessageBox.Show("Нельзя удалить водителя, у которого есть рейсы.");
                    return;
                }
                var drivDeleted = db.Drivers.Find(driver.Id);
                db.Drivers.Remove(drivDeleted);
                db.SaveChanges();
                LoadDrivers();
            }
        }


    }

}
