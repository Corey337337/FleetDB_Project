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
using FleetDB_Project.VehiclesFiles;

namespace FleetDB_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для VehiclesPage.xaml
    /// </summary>
    public partial class VehiclesPage : Page
    {
        public VehiclesPage()
        {
            InitializeComponent();
            LoadStatus();
            LoadVehicles();

        }

        public void LoadStatus()
        {
            using (var db = new FleetDBEntities())
            {
                var statuses = db.Vehicles.Select(x => x.Status).
                    Distinct().OrderBy(x => x).ToList();
                statuses.Insert(0, "Все");
                StatusBox.ItemsSource = statuses;
                StatusBox.SelectedIndex = 0;
            }
        }

        public void LoadVehicles()
        {
            try
            {
                string search = SearchBox.Text.Trim();

                using (var db = new FleetDBEntities())
                {
                    var vehicles = db.Vehicles.AsQueryable();


                    if (!string.IsNullOrEmpty(search))
                    {
                        vehicles = vehicles.Where(x => x.Model.Contains(search));
                    }

                    if (StatusBox.SelectedIndex > 0)
                    {
                        string selectedStatus = StatusBox.SelectedItem.ToString();
                        vehicles = vehicles.Where(x => x.Status == selectedStatus);
                    }

                    switch (SortBox.SelectedIndex)
                    {
                        case 0:
                            vehicles = vehicles.OrderBy(x => x.Model).ThenBy(c => c.Mileage);
                            break;
                        case 1:
                            vehicles = vehicles.OrderByDescending(x => x.Model).ThenBy(c => c.Mileage);
                            break;
                        case 2:
                            vehicles = vehicles.OrderBy(x => x.Mileage);
                            break;
                        case 3:
                            vehicles = vehicles.OrderByDescending(x => x.Mileage);
                            break;
                    }



                    CarsGrid.ItemsSource = vehicles.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось получить транспортные средства! Проверьте подключение к БД! {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }



        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            LoadVehicles();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            SortBox.SelectedIndex = 0;
            StatusBox.SelectedIndex = 0;
            LoadVehicles();
        }

        private void AddNewVehicle_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewVehiclePage());
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var v = button?.DataContext as Vehicles;

            NavigationService.Navigate(new VehiclesFiles.EditVehiclePage(v));
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var v = button?.DataContext as Vehicles;

            if (v == null)
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

                if (db.Routes.Any(r => r.VehicleId == v.Id))
                {
                    MessageBox.Show("Нельзя удалить Транспортное Средство, находящееся в рейсе");
                    return;
                }

                var vDeleted = db.Vehicles.Find(v.Id);
                db.Vehicles.Remove(vDeleted);
                db.SaveChanges();
                LoadVehicles();
            }
        }


    }
}
