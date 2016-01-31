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
    /// Логика взаимодействия для WindowOpenComposition.xaml
    /// </summary>
    public partial class WindowOpenComposition : Window
    {
        public WindowOpenComposition()
        {
            InitializeComponent();
        }
        public WindowOpenComposition(User User)
        {
            InitializeComponent();
            CurrentUser = User;
        }

        User CurrentUser;
        Controller Controller;
        public Composition Composition;
        public string Title { get; set; }

        public string Subtitle
        {
            set
            {
                tb_Subtitle.Text = ((Composition)lb_Compositions.SelectedItem).Subtitle;
            }
        }
        
        public string Artist
        {
            get
            {
               return (((Composition)lb_Compositions.SelectedItem).Artist).ToString();

            }
            set
            {
                tb_Artist.Text = (((Composition)lb_Compositions.SelectedItem).Artist).ToString();
            }
            //get
            //{
            //    return (Artist)cb_Artists.SelectedItem;
            //}
            //set
            //{
            //    cb_Artists.SelectedItem = value;
            //}
        }
        public string Album
        {
            get
            {
              return (((Composition)lb_Compositions.SelectedItem).Album).ToString();
            }
            set
            {
                tb_Artist.Text = (((Composition)lb_Compositions.SelectedItem).Album).ToString();
            }
            //set
            //{
            //    cb_Albums.SelectedItem = (List<Album>)((Artist.Find(Artist.LastName)).AlbumsOfArtist());
            //}
            //get
            //{
            //    return (Album)cb_Albums.SelectedItem;
            //}
        }
        public string Words
        {
            set
            {
                tb_Words.Text = ((Composition)lb_Compositions.SelectedItem).Words;
            }
        }
        public string Music
        {
            set
            {
                tb_Music.Text = ((Composition)lb_Compositions.SelectedItem).Music;
            }
        }

        public string Tabs
        {
            set
            {
                tb_Tabs.Text = ((Composition)lb_Compositions.SelectedItem).Tabs;
            }
        }
        public string Copyright
        {
            set
            {
                tb_Copyright.Text = ((Composition)lb_Compositions.SelectedItem).Copyright;
            }
        }
        public string Notice
        {

            set
            {
                tb_Notice.Text = ((Composition)lb_Compositions.SelectedItem).Notice;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new Controller(); ;
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void lb_Compositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Composition = (Composition)lb_Compositions.SelectedItem;

         //    Title = Composition.Title;
        //    tb_Title.Text = Title;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl TabControl = sender as TabControl;
            TabItem SelectedTabItem = TabControl.SelectedItem as TabItem;
            if (SelectedTabItem.Header.ToString() == "My")
                lb_Compositions.ItemsSource = CurrentUser.Compositions();
            if (SelectedTabItem.Header.ToString() == "Added")
                lb_Compositions.ItemsSource = CurrentUser.AddedCompositions();
            if (SelectedTabItem.Header.ToString() == "All")
                lb_Compositions.ItemsSource = Controller.ShowCompositions();
        }
    }
}