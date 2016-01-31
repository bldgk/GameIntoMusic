using GiM.Classes.Data_Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiM.Classes.Programm_Classes;

namespace GiM.Classes
{
    [Serializable]
    [ClassInfo(true)]
    public class Composition : Base
    {
        #region Properties
        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, PrimaryKey = true)]
        public Guid Id { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = true)]
        public string Title { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Subtitle { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Words { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Music { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Tabs { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Copyright { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Notice { get; set; }
        //public DateTime DateCreated { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, UniqueKey = false)]
        public Guid AlbumId
        {
            get
            {
                return this.Album.Id;
            }
            set
            {
                this.AlbumId = (Guid)((Album)Album.FindThisInstanceInDB((Guid)value)).Id;
                this.Album = (Album)Album.FindThisInstanceInDB(this.AlbumId);
            }
        }

        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, UniqueKey = false)]
        public Guid ArtistId
        {
            get
            {
                return this.Artist.Id;
            }
            set
            {
                this.ArtistId = (Guid)((Artist)Artist.FindThisInstanceInDB((Guid)value)).Id;
                this.Artist = (Artist)Artist.FindThisInstanceInDB(this.ArtistId);
            }
        }

        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, UniqueKey = false)]
        public Guid AuthorId
        {
            get
            {
                return this.Author.Id;
            }
            set
            {
                this.AuthorId = (Guid)((User)User.Find((Guid)value)).Id;
                this.Author = (User)User.Find(this.AuthorId);
            }
        }

        public Album Album { get; set; }
        public Artist Artist { get; set; }
        public User Author { get; set; }

        public ICollection<Track> TracksInComposition()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Track> TracksInComposition = new List<Track>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = @"
                            SELECT * 
                            FROM Tracks
                            WHERE CompositionId = @compositionid";
                sqlcmd.Parameters.AddWithValue("@compositionid", this.Id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            Track Track = new Track();
                            Track = new Track();
                            Track.Id = (Guid)sqlreader[0];
                            Track.FullName = (string)sqlreader[1];
                            Track.ShortName = (string)sqlreader[2];
                            Track.Color = (string)sqlreader[3];
                            //Track.Family = (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]);
                            //Track.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]);
                            //Track.Feature = (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[5]);
                            Track.Family = (string)sqlreader[4];
                            Track.Type = (string)sqlreader[5];
                            Track.Feature = (string)sqlreader[6];
                            Track.Composition = Composition.Find((Guid)sqlreader[7]);
                            Track.Instrument = new Instrument((InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]), (InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]), (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[6]));

                            TracksInComposition.Add(Track);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return TracksInComposition;
                }
            }
        }          
        #endregion

        #region Constructors

        public Composition()
        {
            this.Title = "";
            this.Subtitle = "";
            this.Words = "";
            this.Music = "";
            this.Tabs = "";
            this.Copyright = "";
            this.Notice = "";
            //this.DateCreated = DateTime.Now;
            this.Album = null;
            this.Artist = null;
            this.Author = null;
        }

        public Composition(string title, string subtitle, string words, string music, string tabs, string copyright, string notice, Album album, Artist artist, User author)//, DateTime datecreated)
        {
            this.Id = Guid.NewGuid();
            this.Title = title;
            this.Subtitle = subtitle;
            this.Words = words;
            this.Music = music;
            this.Tabs = tabs;
            this.Copyright = copyright;
            this.Notice = notice;
            //this.DateCreated = datecreated;
            this.Album = album;
            this.Artist = artist;
            this.Author = author;
            this.Save();
        }
        #endregion

        #region Methods
        
        public override string ToString()
        {
            return Title;

        }


        /// <summary>
        /// Search by name
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Composition Find(String title)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Composition Composition = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Compositions WHERE Title=@title";
                sqlcmd.Parameters.AddWithValue("@title", title);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Composition = new Composition();
                        Composition.Id = (Guid)sqlreader[0];
                        Composition.Title = (string)sqlreader[1];
                        Composition.Subtitle = (string)sqlreader[2];
                        Composition.Words = (string)sqlreader[3];
                        Composition.Music = (string)sqlreader[4];
                        Composition.Tabs = (string)sqlreader[5];
                        Composition.Copyright = (string)sqlreader[6];
                        Composition.Notice = (string)sqlreader[7];
                        Composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[8]);
                        Composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[9]);
                        Composition.Author = (User)User.Find((Guid)sqlreader[10]);
                        //Composition.DateCreated = (DateTime)sqlreader[9];
                    }
                    sqlcmd.Connection.Close();
                    return Composition;
                }
            }
        }

        /// <summary>
        /// Search by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Composition Find(Guid id)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Composition Composition = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Compositions WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {

                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Composition = new Composition();
                        Composition = new Composition();
                        Composition.Id = (Guid)sqlreader[0];
                        Composition.Title = (string)sqlreader[1];
                        Composition.Subtitle = (string)sqlreader[2];
                        Composition.Words = (string)sqlreader[3];
                        Composition.Music = (string)sqlreader[4];
                        Composition.Tabs = (string)sqlreader[5];
                        Composition.Copyright = (string)sqlreader[6];
                        Composition.Notice = (string)sqlreader[7];
                        Composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[8]);
                        Composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[9]);
                        Composition.Author = (User)User.Find((Guid)sqlreader[10]);
                        //Composition.DateCreated = (DateTime)sqlreader[9];

                    }
                    sqlcmd.Connection.Close();
                    return Composition;
                }
            }
        }

        /// <summary>
        /// Loading of all instances from database
        /// </summary>
        /// <returns></returns>
        public static List<Composition> Load()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Composition> ListCompositions = new List<Composition>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Compositions";
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            Composition Composition = new Composition();
                            Composition = new Composition();
                            Composition.Id = (Guid)sqlreader[0];
                            Composition.Title = (string)sqlreader[1];
                            Composition.Subtitle = (string)sqlreader[2];
                            Composition.Words = (string)sqlreader[3];
                            Composition.Music = (string)sqlreader[4];
                            Composition.Tabs = (string)sqlreader[5];
                            Composition.Copyright = (string)sqlreader[6];
                            Composition.Notice = (string)sqlreader[7];
                            Composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[8]);
                            Composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[9]);
                            Composition.Author = (User)User.Find((Guid)sqlreader[10]);
                            //Composition.DateCreated = (DateTime)sqlreader[9];

                            //album.DateReleased = Convert.ToDateTime((string)sqlreader[2]);
                            ListCompositions.Add(Composition);

                        }
                    }
                    sqlcmd.Connection.Close();
                    return ListCompositions;
                }
            }
        #endregion
        }
    }
}