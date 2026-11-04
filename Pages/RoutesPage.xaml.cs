using FleetDB_Project.VehiclesFiles;
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
using FleetDB_Project.RoutesFiles;

namespace FleetDB_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для RoutesPage.xaml
    /// </summary>
    public partial class RoutesPage : Page
    {
        public RoutesPage()
        {
            InitializeComponent();
            LoadDrivers();
            LoadRoutes();
        }


        public void LoadDrivers()
        {
            using (var db = new FleetDBEntities())
            {
                var drivers = db.Drivers.Select(x => x.Id).Distinct()
                    .OrderBy(x => x)
                    .ToList()
                    .Select(x => x.ToString())
                    .ToList();
                drivers.Insert(0, "Все водители");
                DriversBox.ItemsSource = drivers;
                DriversBox.SelectedIndex = 0;
            }
        }

        public void LoadRoutes()
        {

            try
            {
                string search = SearchBox.Text.Trim();

                using (var db = new FleetDBEntities())
                {

                    var routes = db.Routes.AsQueryable();

                    if (!string.IsNullOrEmpty(search))
                    {
                        routes = routes.Where(x => x.EndLocation.Contains(search));
                    }

                    if (DriversBox.SelectedIndex > 0)
                    {
                        int selectedDriverId = int.Parse(DriversBox.SelectedItem.ToString());
                        routes = routes.Where(x => x.DriverId == selectedDriverId);
                    }

                    switch (SortBox.SelectedIndex)
                    {
                        case 0:
                            routes = routes.OrderBy(x => x.StartDate).ThenBy(c => c.Distance);
                            break;
                        case 1:
                            routes = routes.OrderByDescending(x => x.StartDate).ThenBy(c => c.Distance);
                            break;
                        case 2:
                            routes = routes.OrderBy(x => x.Distance);
                            break;
                        case 3:
                            routes = routes.OrderByDescending(x => x.Distance);
                            break;
                    }

                    RoutesGrid.ItemsSource = routes.ToList();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось получить маршруты! Проверьте подключение к БД! {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }

        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            var route = (sender as Button).DataContext as Routes;

            using (var db = new FleetDBEntities())
            {
                var r = db.Routes.Find(route.Id);
                r.EndDate = DateTime.Now;

                var vehicle = db.Vehicles.Find(r.VehicleId);
                vehicle.Status = "Доступен";

                db.SaveChanges();
            }

            LoadRoutes();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            LoadRoutes();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            SortBox.SelectedIndex = 0;
            DriversBox.SelectedIndex = 0;
            LoadRoutes();
        }

        private void AddNewRoute_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewRoutePage());
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var route = button?.DataContext as Routes;

            NavigationService.Navigate(new RoutesFiles.EditRoutePage(route));
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var route = button?.DataContext as Routes;

            if (route == null)
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
                var routeDeleted = db.Routes.Find(route.Id);
                db.Routes.Remove(routeDeleted);
                db.SaveChanges();
                LoadRoutes();
            }

        }
    }
}
