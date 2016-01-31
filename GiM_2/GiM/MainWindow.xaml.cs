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
using System.Data.Entity;

using GiM.Classes;
using GiM.Classes.Data_Classes;
using GiM.Classes.Programm_Classes;
namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        WindowProfile WindowProfile;
        WindowRegistration WindowRegistration;
        
        private void mi_ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            WindowProfile = new WindowProfile();
            WindowProfile.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void New_composition_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Registration_Click(object sender, RoutedEventArgs e)
        {
            WindowRegistration = new WindowRegistration();
            WindowRegistration.Show();
        }

    }
}
