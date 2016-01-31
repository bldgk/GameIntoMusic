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
    /// Логика взаимодействия для WindowAddComposition.xaml
    /// </summary>
    public partial class WindowAddComposition : Window
    {
        public WindowAddComposition()
        {
            InitializeComponent();

        }

        WindowAddArtist WindowAddArtist;
        WindowAddAlbum WindowAddAlbum;
        Controller Controller;
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(tb_Title.Text))
                    return tb_Title.Text;
                else return "New Composition " + DateTime.Now.ToString();
            }
            set
            {
                tb_Title.Text = value;
            }
        }

        public string Subtitle
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Subtitle.Text))
                    return "";
                else return tb_Subtitle.Text;
            }
            set
            {
                tb_Subtitle.Text = value;
            }
        }

        public Artist Artist
        {
            get
            {
                return (Artist)cb_Artists.SelectedItem;
            }
        }
        public Album Album
        {
            get
            {
                try
                {
                    return (Album)cb_Albums.SelectedItem;
                }
                catch { return null; }
            }
        }
        public string Words
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Words.Text))
                    return "";
                else return tb_Words.Text;
            }
            set
            {
                tb_Words.Text = value;
            }
        }
        public string Music
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Music.Text))
                    return "";
                else return tb_Music.Text;
            }
            set
            {
                tb_Music.Text = value;
            }
        }

        public string Tabs
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Tabs.Text))
                    return "";
                else return tb_Tabs.Text;
            }
            set
            {
                tb_Tabs.Text = value;
            }
        }
        public string Copyright
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Copyright.Text))
                    return "";
                else return tb_Copyright.Text;
            }
            set
            {
                tb_Copyright.Text = value;
            }
        }
        public string Notice
        {
            get
            {
                if (string.IsNullOrEmpty(tb_Words.Text))
                    return "";
                else return tb_Notice.Text;
            }
            set
            {
                tb_Notice.Text = value;
            }
        }
        private void WindowAddComposition1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_AddArtist_Click(object sender, RoutedEventArgs e)
        {
            WindowAddArtist = new WindowAddArtist();
            WindowAddArtist.Owner = this;
            WindowAddArtist.ShowDialog();
            if (WindowAddArtist.DialogResult.HasValue && WindowAddArtist.DialogResult.Value)
            {
                Controller.CreateArtist(WindowAddArtist.LastName, WindowAddArtist.FirstName);
            }
            RefreshComboboxes();

        }

        private void btn_AddAlbum_Click(object sender, RoutedEventArgs e)
        {
            WindowAddAlbum = new WindowAddAlbum();
            WindowAddAlbum.Owner = this;
            WindowAddAlbum.ShowDialog();
            if (WindowAddAlbum.DialogResult.HasValue && WindowAddAlbum.DialogResult.Value)
            {
                Controller.CreateAlbum(WindowAddAlbum.Title, (Artist)cb_Artists.SelectedItem);
            }
            RefreshComboboxes();
        }

        private void WindowAddComposition1_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new Controller();
            RefreshComboboxes();
        }
        public void RefreshComboboxes()
        {
            cb_Artists.ItemsSource = Controller.ShowArtists();
           // cb_Albums.ItemsSource = Albums;
        }

        public void cb_Artists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cb_Albums.ItemsSource = (List<Album>)((Artist)cb_Artists.SelectedItem).AlbumsOfArtist();
            }
            catch { }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
