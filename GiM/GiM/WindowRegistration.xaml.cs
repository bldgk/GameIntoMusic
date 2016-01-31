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
using System.Security.Cryptography;
using System.Net.Mail;
using GiM.Classes.Data_Classes;
using GiM.Classes.Programm_Classes;

namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для WindowRegistration.xaml
    /// </summary>
    public partial class WindowRegistration : Window
    {
        public WindowRegistration()
        {
            InitializeComponent();

        }

        Controller Controller;
        public static string Digit { get; set; }
        public static WindowRegistration Instance { get { return _instance ?? (_instance = new WindowRegistration()); } }
        private static WindowRegistration _instance;
        User User;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new Controller();

        }

        private void btn_Perform_Reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tb_first_name.Text) || string.IsNullOrEmpty(tb_last_name.Text) || string.IsNullOrEmpty(tb_email.Text) || string.IsNullOrEmpty(tb_password.Text) || string.IsNullOrEmpty(tb_repassword.Text))
                {
                    if (tb_password.Text == tb_password.Text)
                    {
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                        UTF8Encoding utf8 = new UTF8Encoding();
                        string TempNickName = tb_user_name.Text;
                        if (string.IsNullOrEmpty(tb_user_name.Text))
                            TempNickName = tb_first_name.Text + " " + tb_last_name.Text;
                        Controller.CreateUser(tb_first_name.Text, tb_last_name.Text, TempNickName, tb_password.Text, tb_email.Text);


                    }
                }
                #region Comment
                /*
                //var rnd = new Random();
                //        if (tb_password.Text != tb_repassword.Text) { MessageBox.Show("Password don't match!"); }

                //        if (tb_password.Text == tb_repassword.Text)
                //        {

                //            SmtpClient client = new SmtpClient("smtp.gmail.com");
                //            MailMessage Message = new MailMessage("WelcomeToNCI@gmail.com", tb_email.Text);
                //            Message.Subject = "NCI";
                //            Digit = Convert.ToString(rnd.Next(10000));
                //            Message.Body = "Welcome to NCI!\nDear " + tb_first_name.Text + "  " + tb_last_name.Text + " your username is: " + tb_user_name.Text
                //                + "\n\n your password is: " + tb_password.Text + "\n\n Thank you for singing up!" + "\n\n Enter this digit - " + Digit;
                //            client.Host = "smtp.gmail.com";
                //            client.Port = 587;
                //            client.EnableSsl = true;
                //            client.Credentials = new System.Net.NetworkCredential("WelcomeToNCI@gmail.com", "bushido777");
                //            client.Send(Message);
                //            MessageBox.Show("Messages sent!");
                //            tb_first_name.Clear();
                //            tb_last_name.Clear();
                //            tb_user_name.Clear();
                //            tb_email.Clear();
                //            tb_password.Clear();
                //            tb_repassword.Clear();
                //            User = new User(tb_first_name.Text, tb_last_name.Text, nickname, tb_email.Text, BitConverter.ToString(md5.ComputeHash(utf8.GetBytes(tb_password.Text))));
                //        }
                //    }
                 * */
                #endregion
                this.Close();
            }
            catch { }

        }
        private void WindowRegistration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
          
        }

    }
}