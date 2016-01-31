using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GiM.Classes.Data_Classes;

using System.Reflection;


namespace GiM.Classes.Programm_Classes
{
    public class Controller
    {
        public void Create(Type objtype)// where T:class
        {
            Type ObjectType = objtype;
            MethodInfo[] Methods = ObjectType.GetMethods();
            if (ObjectType != null)
            {
                ConstructorInfo Constructors = ObjectType.GetConstructor(new Type[] { });
                object NewObject = Constructors.Invoke(new object[] { });
            }
        }
        public void CreateUser()
        {
            User User = new User();
        }

        public void CreateArtist()
        {
            Artist Artist = new Artist();
        }

        public void CreateAlbum()
        {
            Album Album = new Album();
        }

        public void CreateComposition()
        {
            Composition Composition = new Composition();
        }

        public void CreateTrack()
        {
            Track Track = new Track();
        }
        
        public void CreateUser(string FirstName, string LastName, string UserName, string Password, string Email)
        {
            User user = new User(FirstName, LastName, UserName, Email, Password);
            user.LocalSave();
        }

        public void CreateArtist(string LastName, string FirstName)
        {
            Artist Artist = new Artist(LastName, FirstName);
        }

        public void CreateAlbum(string Name, Artist SelectedArtist)
        {
            Album alb = new Album(Name, SelectedArtist);
        }

        public void CreateComposition(string Title, string Subtitle, string Words, string Music, string Tabs, string Copyright, string Notice, Album SelectedAlbum, Artist SelectedArtist, User SelectedAuthor)
        {
            Composition Composition = new Composition(Title, Subtitle, Words, Music, Tabs, Copyright, Notice, SelectedAlbum, SelectedArtist, SelectedAuthor);
       //     UserComposition UserComposition = new UserComposition(SelectedAuthor, Composition);
        }

        public void CreateTrack(Composition SelectedComposition, string FullName, string ShortName, InstrumentFamily Family, InstrumentType Type, InstrumentFeature Feature, string Color)
        {
            Track Track = new Track(SelectedComposition, FullName, ShortName, Family, Type, Feature, Color);
        }

        public void EditArtist(Artist SelectedArtist, string NewFirstName, string NewLastName)
        {
            SelectedArtist.Update(new Artist() { FirstName = NewFirstName, LastName = NewLastName });
        }

        public void EditUser(User SelectedUser, string NewFirstName, string NewLastName, string NewUserName, string NewPassword, string NewEmail)
        {
            SelectedUser.Update(
                new User()
                {
                    FirstName = NewFirstName,
                    LastName = NewLastName,
                    UserName = NewUserName,
                    Password = NewPassword,
                    Email = NewEmail
                });
        }

        public void EditComposition(Composition SelectedComposition, string NewTitle, string NewSubtitle, string NewWords, string NewMusic, string NewTabs, string NewCopyright, string NewNotice, Album NewSelectedAlbum, Artist NewSelectedArtist, User NewAuthor)
        {
            SelectedComposition.Update(
                new Composition()
            {
                Title = NewTitle,
                Subtitle = NewSubtitle,
                Words = NewWords,
                Music = NewMusic,
                Tabs = NewTabs,
                Copyright = NewCopyright,
                Notice = NewNotice,
                Album = NewSelectedAlbum,
                Artist = NewSelectedArtist,
                Author = NewAuthor
            });
        }

        public void EditTrack(Track SelectedTrack, Composition NewComposition, string NewFullName, string NewShortName, InstrumentFamily NewFamily, InstrumentType NewType, InstrumentFeature NewFeature, string NewColor)
        {
            SelectedTrack.Update(
                new Track()
        {
            Composition = NewComposition,
            FullName = NewFullName,
            ShortName = NewShortName,
            Family = NewFamily.ToString(),
            Type = NewType.ToString(),
            Feature = NewFeature.ToString(),
            Instrument = new Instrument(NewFamily, NewType, NewFeature),
            Color = NewColor
        });
        }


        public void DeleteArtist(Artist SelectedArtist)
        {
            ICollection<Album> Albs = new List<Album>();
            foreach (Album alb in Album.Load())
            {
                if (alb.Artist.Id == SelectedArtist.Id)
                    Albs.Add(alb);
            }
            if (Albs.Count == 0)
                SelectedArtist.Delete();
        }

        public void DeleteAlbum(Album SelectedAlbum)
        {
            ICollection<Composition> compositions = new List<Composition>();
            foreach (Composition comp in Composition.Load())
            {
                if (comp.Artist.Id == SelectedAlbum.Id)
                    compositions.Add(comp);
            }
            if (compositions.Count == 0)
                SelectedAlbum.Delete();
        }

        public void DeleteComposition(Composition SelectedComposition)
        {
            ICollection<Track> Tracks = new List<Track>();
            foreach (Track CurrentTrack in Track.Load())
            {
                if (CurrentTrack.Composition.Id == SelectedComposition.Id)
                    Tracks.Add(CurrentTrack);
            }
            foreach (Track CurrentTrack in Tracks)
                CurrentTrack.Delete();
            SelectedComposition.Delete();
        }

        public void DeleteTrack(Track SelectedTrack)
        {
            SelectedTrack.Delete();
        }

        public void CreateLink(User SelectedUser, Composition SelectedComposition)
        {
            UserComposition UserComposition = new UserComposition(SelectedUser, SelectedComposition);
        }
        public ICollection<Artist> ShowArtists()
        {
            return Artist.LoadAllFromDB();
        }
        public ICollection<Composition> ShowCompositions()
        {
            return Composition.Load();
        }

        public Composition FindComposition(string Title)
        {
            return Composition.Find(Title);
        }
        public Track FindTrack(string FullName)
        {
            return Track.Find(FullName);
        }

    }
}