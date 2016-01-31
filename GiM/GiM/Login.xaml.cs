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
using GiM.Classes.Programm_Classes;
using GiM.Classes.Data_Classes;
using GiM.Classes;

namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        public User LoginingUser { get; set; }

        //public string FirstName
        //{
        //    get
        //    {
        //        return tb_first_name.Text;
        //    }
        //    set
        //    {
        //        tb_first_name.Text = value;
        //    }
        //}
        //public string LastName
        //{
        //    get
        //    {
        //        return tb_last_name.Text;
        //    }
        //    set
        //    {
        //        tb_last_name.Text = value;
        //    }
        //}

        //public string UserName
        //{
        //    get
        //    {
        //        return tb_user_name.Text;
        //    }
        //    set
        //    {
        //        tb_user_name.Text = value;
        //    }
        //}


        public string Email
        {
            get
            {
                return tb_email.Text;
            }
            set
            {
                tb_email.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return tb_password.Text;
            }
            set
            {
                tb_password.Text = value;
            }
        }



        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (!string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(Password))
            {
                foreach (User user in User.LocalUsers)
                {
                    if (user.Email == Email && user.Password == Password)
                    {
                        LoginingUser = User.Find(Email);
                        break;
                    }
                }
            }

        }

        private void WindowLogin_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WindowLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
