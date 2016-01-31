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
using GiM.Classes;
using GiM.Classes.Data_Classes;
using GiM.Classes.Programm_Classes;

namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для WindowInformation.xaml
    /// </summary>
    public partial class WindowInformation : Window
    {
        public WindowInformation()
        {
            InitializeComponent();
        }
        Composition CompositionInformation { get; set; }
        public WindowInformation(Composition SelectedComposition)
        {
            InitializeComponent();
            CompositionInformation = SelectedComposition;
        }

       
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
