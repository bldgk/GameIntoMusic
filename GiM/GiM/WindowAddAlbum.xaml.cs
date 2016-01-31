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
    /// Логика взаимодействия для WindowAddAlbum.xaml
    /// </summary>
    public partial class WindowAddAlbum : Window
    {
        public WindowAddAlbum()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(tb_Title.Text))
                    return tb_Title.Text;
                else
                    return "New Album ";
            }
            set
            {
                tb_Title.Text = value;
            }
        }

        public string DateOfReleased
        {
            get
            {
                if (!string.IsNullOrEmpty(dp_Date.Text))
                    return dp_Date.Text;
                else return DateTime.Now.ToString();
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

        private void WindowAddAlbum_MouseDown(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }
    }
}
