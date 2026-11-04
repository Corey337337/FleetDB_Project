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
using System.Windows.Shapes;
using FleetDB_Project.Pages;

namespace FleetDB_Project.Windows
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home(string email)
        {
            InitializeComponent();
        }
        
        private void DriversPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DriversPage());
        }

        private void VehiclesPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VehiclesPage());
        }

        private void RoutesPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RoutesPage());
        }
    }
}
