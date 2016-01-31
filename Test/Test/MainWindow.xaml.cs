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
using GiM.Classes;
using GiM.Classes.Data_Classes;
using GiM.Classes.Programm_Classes;

namespace Test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // User1 = new User("Kirill", "Beldyaga", "Super_zhmel", "kirill.beldyaga@gmail.com", "qwrq");
            var families = new List<string>();
            families.Add(InstrumentFamily.family1.ToString());
            families.Add(InstrumentFamily.family2.ToString());
            families.Add(InstrumentFamily.family3.ToString());
            var types = new List<string>();
            types.Add(InstrumentType.type1.ToString());
            types.Add(InstrumentType.type2.ToString());
            types.Add(InstrumentType.type3.ToString());
            var features = new List<string>();
            features.Add(InstrumentFeature.feature1.ToString());
            features.Add(InstrumentFeature.feature2.ToString());
            features.Add(InstrumentFeature.feature3.ToString());
            cb_Family.ItemsSource = families;
            cb_Type.ItemsSource = types;
            cb_Feature.ItemsSource = features;
        }

        Controller Controller = new Controller();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Controller.CreateArtist(tb_ln.Text, tb_fn.Text);
            Controller.Create(Type.GetType("Artist"));
        }

        private void btn_add_album_Click(object sender, RoutedEventArgs e)
        {
            Controller.CreateAlbum(tb_title.Text, (Artist)Artists.SelectedItem);
        }

        private void btn_add_title_Click(object sender, RoutedEventArgs e)
        {
            Controller.CreateComposition(tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, (Album)Albums.SelectedItem, (Artist)Artists.SelectedItem, (User)Users.SelectedItem);
        }

        private void buton_add_trak_Click(object sender, RoutedEventArgs e)
        {
            Controller.CreateTrack((Composition)Compositions.SelectedItem, tb_add_track.Text, tb_add_track.Text, (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), cb_Family.SelectedItem.ToString()), (InstrumentType)Enum.Parse(typeof(InstrumentType), cb_Type.SelectedItem.ToString()), (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), cb_Feature.SelectedItem.ToString()), tb_add_track.Text);
        }

        private void btn_show_Click(object sender, RoutedEventArgs e)
        {
            Users.ItemsSource = User.Load();
            Artists.ItemsSource = Artist.LoadAllFromDB();
            Albums.ItemsSource = Album.Load();
            Compositions.ItemsSource = Composition.Load();

            Tracks.ItemsSource = Track.Load();
            //; Compositions.ItemsSource = db.Compositions.ToList();

            //   Tracks.ItemsSource = db.Tracks.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            AlbumArtist.ItemsSource = (List<Album>)((Artist)Artists.SelectedItem).AlbumsOfArtist();
        }




        private void btn_compsinalb_Click(object sender, RoutedEventArgs e)
        {
            CompAlbum.ItemsSource = (List<Composition>)((Album)Albums.SelectedItem).CompositionsInAlbum();
        }


        private void trackInComp_Click(object sender, RoutedEventArgs e)
        {
            CompsInTracks.ItemsSource = (List<Track>)((Composition)Compositions.SelectedItem).TracksInComposition();
        }

        private void CompOfArt_Click(object sender, RoutedEventArgs e)
        {
            CompOfArtist.ItemsSource = (List<Composition>)((Artist)Artists.SelectedItem).CompositionsOfArtist();
        }

        private void btn_add_user_Click(object sender, RoutedEventArgs e)
        {
            Controller.CreateUser(tb_user.Text, tb_user.Text, tb_user.Text, tb_user.Text, tb_user.Text);

        }

        private void Butom_Click(object sender, RoutedEventArgs e)
        {
            UserComps.ItemsSource = (List<Composition>)(((User)Users.SelectedItem).AddedCompositions());
        }

        private void we_Click(object sender, RoutedEventArgs e)
        {
            UserComposition us = new UserComposition((User)Users.SelectedItem, (Composition)Compositions.SelectedItem);

        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
           // Controller.EditArtist((Artist)Artists.SelectedItem, tb_fn.Text, tb_ln.Text);
            //Controller.EditUser((User)Users.SelectedItem, tb_user.Text, tb_user.Text, tb_user.Text, tb_user.Text, tb_user.Text);
          //  Controller.EditComposition((Composition)Compositions.SelectedItem, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, tb_comp_title.Text, (Album)Albums.SelectedItem, (Artist)Artists.SelectedItem, (User)Users.SelectedItem);

            Controller.EditTrack((Track)Tracks.SelectedItem,(Composition)Compositions.SelectedItem, tb_add_track.Text, tb_add_track.Text, (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), cb_Family.SelectedItem.ToString()), (InstrumentType)Enum.Parse(typeof(InstrumentType), cb_Type.SelectedItem.ToString()), (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), cb_Feature.SelectedItem.ToString()), tb_add_track.Text);
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            //Controller.DeleteArtist((Artist)(Artists.SelectedItem));
            Controller.DeleteComposition((Composition)Compositions.SelectedItem);
            //Controller.DeleteAlbum((Album)(Albums.SelectedItem));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Note nt = new Note(DegreeNote.C, TypeNote.Quarter, OctaveNote.First, AlterationNote.None);
            (((Track)Tracks.SelectedItem).Notes = new List<Note>()).Add(nt);
            nt = new Note(DegreeNote.D, TypeNote.Quarter, OctaveNote.First, AlterationNote.None);
            ((Track)Tracks.SelectedItem).Notes.Add(nt);
            res.Text = nt.Name;
            traks.Text = ((Track)Tracks.SelectedItem).NotesInTrack;
        }
    }
}