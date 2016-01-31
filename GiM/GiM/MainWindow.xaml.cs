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
using System.Runtime.Serialization;
using System.Xml;

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
        #region Properties
        #region Windows
        WindowProfile WindowProfile;
        WindowRegistration WindowRegistration;
        WindowAddComposition WindowAddComposition;
        WindowOpenComposition WindowOpenComposition;
        WindowAddTrack WindowAddTrack;
        #endregion
        BrushConverter BrushConverter = new BrushConverter();
        [DataMember]
        public User CurrentUser { get { return User; } set { User = value; } }
        bool Signing { get { if (!string.IsNullOrEmpty(CurrentUser.Email)) return true; else return false; } }
        User User = new User();
        Controller Controller;
        Composition Composition;
        Track Track;
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new Controller();
            this.LoadSettings();
            if (string.IsNullOrEmpty(User.Email))
                mi_Log.Header = "Sign in";

        }

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
            WindowAddComposition = new WindowAddComposition();
            WindowAddComposition.Owner = this;
            WindowAddComposition.ShowDialog();
            if (WindowAddComposition.DialogResult.HasValue && WindowAddComposition.DialogResult.Value)
            {
                Controller.CreateComposition(WindowAddComposition.Title, WindowAddComposition.Subtitle, WindowAddComposition.Words, WindowAddComposition.Music, WindowAddComposition.Tabs, WindowAddComposition.Copyright, WindowAddComposition.Notice, WindowAddComposition.Album, WindowAddComposition.Artist, User);
                Composition = Controller.FindComposition(WindowAddComposition.Title);
                TabItem ti = new TabItem();
                ti.Header = Composition.Title;
                ti.IsSelected = true;
                TabCompositions.Items.Add(ti);
            }
        }
        private void mi_Registration_Click(object sender, RoutedEventArgs e)
        {
            WindowRegistration = new WindowRegistration();
            WindowRegistration.Show();
        }

        private void mi_File_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MessageBox.Show("er");
        }

        private void mi_Track_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void mi_AddTrack_Click(object sender, RoutedEventArgs e)
        {
            WindowAddTrack = new WindowAddTrack();
            WindowAddTrack.Owner = this;
            WindowAddTrack.ShowDialog();
            if (WindowAddTrack.DialogResult.HasValue && WindowAddTrack.DialogResult.Value)
            {
                Controller.CreateTrack(Composition, WindowAddTrack.FullName, WindowAddTrack.ShortName, WindowAddTrack.Family, WindowAddTrack.Type, WindowAddTrack.Feature, "Red");
                Track = Controller.FindTrack(WindowAddTrack.FullName);
            }
            TabItem ti = new TabItem();
            ti.Header = Track.ShortName;
            ti.IsSelected = true;
            TabTracks.Items.Add(ti);

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void mi_Log_Click(object sender, RoutedEventArgs e)
        {
            if (!Signing)
            {
                Login Login = new Login();
                Login.Owner = this;

                Login.ShowDialog();
                if (Login.DialogResult.HasValue && Login.DialogResult.Value)
                {
                    User = Login.LoginingUser;
                    mi_Log.Header = "Log out";
                    mi_Account.Header = User.UserName;
                }
            }
            else if (Signing)
            {
                User = new User();
                mi_Log.Header = "Log in";
                
                mi_Account.Header = "Log in!";
            }
        }

        private void mi_Settings_Click(object sender, RoutedEventArgs e)
        {
           
        }

        public void SaveSettings()
        {
            try
            {
                List<User> Saves = new List<Classes.User>();
                Saves.Add(CurrentUser);
                DataContractSerializer xmls = new DataContractSerializer(typeof(List<User>));
                XmlWriter xmlw = XmlWriter.Create("Settings.xml");
                xmls.WriteObject(xmlw, Saves);
                xmlw.Close();
            }
            catch { }
        }


        public void LoadSettings()
        {


            try
            {
                User.LocalLoad();
                List<User> Saves = new List<User>();
                DataContractSerializer xmls = new DataContractSerializer(typeof(List<User>));
                XmlReader xmlr = XmlReader.Create("Settings.xml");
                Saves = (List<User>)xmls.ReadObject(xmlr);
                xmlr.Close();
                foreach(var us in Saves)
                    CurrentUser = us;
                mi_Log.Header = "Sign out";
                mi_Account.Header = User.UserName;
            }
            catch { }
        }

        private void mi_Open_Composition_Click(object sender, RoutedEventArgs e)
        {
            WindowOpenComposition = new WindowOpenComposition(CurrentUser);
            WindowOpenComposition.Owner = this;
            WindowOpenComposition.ShowDialog();

            if (WindowOpenComposition.DialogResult.HasValue && WindowOpenComposition.DialogResult.Value)
            {

                Composition = WindowOpenComposition.Composition;
                TabItem ti = new TabItem();
                ti.Header = Composition.Title;
                ti.IsSelected = true;
                TabCompositions.Items.Add(ti);
            }
        }

        private void mi_Information_Click(object sender, RoutedEventArgs e)
        {
            WindowInformation wi = new WindowInformation();
            wi.Show();
        }

        private void mi_Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ni_Go_To_Click(object sender, RoutedEventArgs e)
        {
            WindowGoTo wgt = new WindowGoTo();
            wgt.Show();
        }

        private void mi_Properties_Click(object sender, RoutedEventArgs e)
        {
            WindowTrackInformation wti = new WindowTrackInformation();
            wti.Show();
        }

        private void TabCompositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TabControl TabControl = sender as TabControl;
                TabItem SelectedTabItem = TabControl.SelectedItem as TabItem;
                this.Composition = Composition.Find(SelectedTabItem.Header.ToString());
                TabTracks.Items.Clear();
                foreach (Track NTrack in Composition.TracksInComposition())
                {
                    TabItem ti = new TabItem();
                    ti.Header = NTrack.ShortName;
                    ti.IsSelected = true;
                    TabTracks.Items.Add(ti);
                }
            }
            catch { }
        }

        private void mi_Close_Click(object sender, RoutedEventArgs e)
        {
            var temp = new TabItem();
            foreach (TabItem TableItem in TabCompositions.Items)
                if (TableItem.IsSelected)
                {
                    temp = TableItem;
                    break;
                }
            TabCompositions.Items.Remove(temp);
        }

        private void mi_Save_Click(object sender, RoutedEventArgs e)
        {
          //  Controller.EditComposition(Composition, Composition
        }
        //private void WorkPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var item = sender as TabControl;
        //    // ... Set Title to selected tab header.
        //    var selected = item.SelectedItem as TabItem;
        //    selected.Foreground = (Brush)BrushConverter.ConvertFrom("#FF000000");+		CurrentUser	{}	GiM.Classes.User

        //}
    }
}
