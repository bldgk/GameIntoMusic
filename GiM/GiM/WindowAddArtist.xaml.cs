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

namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для WindowAddArtist.xaml
    /// </summary>
    public partial class WindowAddArtist : Window
    {
        public WindowAddArtist()
        {
            InitializeComponent();
        }

        public string FirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(tb_FirstName.Text))
                    return tb_FirstName.Text;
                else
                    return "The ";
            }
            set
            {
                tb_FirstName.Text = value;
            }
        }



        public string LastName
        {
            get
            {
                if (!string.IsNullOrEmpty(tb_LastName.Text))
                    return tb_LastName.Text;
                else
                    return "New Artist " + DateTime.Now.ToString();
            }
            set
            {
                tb_LastName.Text = value;
            }
        }

        public string Country
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Country.Text))
                    return " ";
                else
                    return tb_Country.Text;
            }
            set
            {
                tb_Country.Text = value;
            }
        }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
