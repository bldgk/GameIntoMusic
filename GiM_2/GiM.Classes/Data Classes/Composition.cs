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
    public class Composition : Base
    {
        #region Properties
        
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Words { get; set; }
        public string Music { get; set; }
        public string Tabs { get; set; }
        public string Copyright { get; set; }
        public string Notice { get; set; }
        public DateTime DateCreated { get; set; }
        public Album Album { get; set; }
        public Artist Artist { get; set; }
        public User User { get; set; }
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
                            Track.Id = (Guid)sqlreader[0];
                            Track.Id = (Guid)sqlreader[0];
                            Track.Composition = (Composition)Composition.FindThisInstanceInDB((Guid)sqlreader[1]);
                            Track.FullName = (string)sqlreader[2];
                            Track.ShortName = (string)sqlreader[3];
                            Track.Family = (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]);
                            Track.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]);
                            Track.Feature = (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[6]);
                            Track.Color = (string)sqlreader[7];

                            TracksInComposition.Add(Track);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return TracksInComposition;
                }
            }
        }          
        public virtual ICollection<Artist> ArtistsOfComposition { get; set; }

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
            //ArtistsOfComposition = new List<Artist>();
            //CompositionInAlbums = new List<Album>();
            this.Album = null;
            this.Artist = null;
        }

        public Composition(string title, string subtitle, string words, string music, string tabs, string copyright, string notice, Album album, Artist artist)//, DateTime datecreated)
        {
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
            ArtistsOfComposition = new List<Artist>();
            //CompositionInAlbums = new List<Album>();
            this.SaveToDB();
        }
        #endregion

        #region Methods
        
        public override string ToString()
        {
            return Title;

        }

        /// <summary>
        /// Existing of such instance in database
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool ExistingOfThisInstance(string title)
        {
            if (FindThisInstanceInDB(title) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Search by name
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Composition FindThisInstanceInDB(String title)
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
                        Composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[1]);
                        Composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[2]);
                        Composition.Title = (string)sqlreader[3];
                        Composition.Subtitle = (string)sqlreader[4];
                        Composition.Words = (string)sqlreader[5];
                        Composition.Music = (string)sqlreader[6];
                        Composition.Tabs = (string)sqlreader[7];
                        Composition.Copyright = (string)sqlreader[8];
                        Composition.Notice = (string)sqlreader[9];
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
        public static Composition FindThisInstanceInDB(Guid id)
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
                        Composition.Id = (Guid)sqlreader[0];
                        Composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[1]);
                        Composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[2]);
                        Composition.Title = (string)sqlreader[3];
                        Composition.Subtitle = (string)sqlreader[4];
                        Composition.Words = (string)sqlreader[5];
                        Composition.Music = (string)sqlreader[6];
                        Composition.Tabs = (string)sqlreader[7];
                        Composition.Copyright = (string)sqlreader[8];
                        Composition.Notice = (string)sqlreader[9];
                        //Composition.DateCreated = (DateTime)sqlreader[9];

                    }
                    sqlcmd.Connection.Close();
                    return Composition;
                }
            }
        }

        public bool DBContains()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                bool contains = false;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Compositions WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", this.Id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        contains = true;
                    }
                    sqlcmd.Connection.Close();
                    return contains;
                }
            }
        }

        /// <summary>
        /// Updating the database
        /// </summary>
        public void UpdateToDB()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "UPDATE Compositions SET AlbumId = '@albumid', ArtistID = '@artistid', Title = '@title', Subtitle = '@subtitle', Words = '@word', Music = '@music', Tabs = '@tabs', Copyright = '@copyright',Notice = '@notice' WHERE Id=@id";
                //     sqlcmd.CommandText = "UPDATE Compositions SET AlbumId = 'albumid', Title = '@title', Subtitle = 'subtitle', Words = 'word, Music = 'music', Tabs = 'tabs', Copyright = 'copyright',Notice = 'notice', DateReleased = 'datereleased WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", this.Id);
                sqlcmd.Parameters.AddWithValue("@albumid", this.Album.Id);
                sqlcmd.Parameters.AddWithValue("@artistid", this.Artist.Id);
                sqlcmd.Parameters.AddWithValue("@title", this.Title);
                sqlcmd.Parameters.AddWithValue("@subtitle", this.Subtitle);
                sqlcmd.Parameters.AddWithValue("@words", this.Words);
                sqlcmd.Parameters.AddWithValue("@music", this.Music);
                sqlcmd.Parameters.AddWithValue("@tabs", this.Tabs);
                sqlcmd.Parameters.AddWithValue("@copyright", this.Copyright);
                sqlcmd.Parameters.AddWithValue("@notice", this.Notice);
                //  sqlcmd.Parameters.AddWithValue("@date_released", this.DateReleased);
                sqlcmd.Connection.Open();
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Connection.Close();
            }
        }

        /// <summary>
        /// Saving to database
        /// </summary>
        public void SaveToDB()
        {
            if (!this.DBContains())
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);

                    sqlcmd.CommandText = "INSERT INTO Compositions(Id, AlbumId, ArtistId, Title, Subtitle, Words, Music, Tabs, Copyright, Notice) values (@id, @albumid, @artistid, @title, @subtitle, @words, @music, @tabs, @copyright, @notice)";
                    //     sqlcmd.CommandText = "INSERT INTO Albums(Id, AlbumId, Title, Subtitle, Words, Music, Tabs, Copyright, Notice, DateReleased) values (@id, @albumid, @title, @subtitle, @words, @music, @tabs, @copyright, @notice, @datereleased)";
                    sqlcmd.Parameters.AddWithValue("@id", this.Id);
                    sqlcmd.Parameters.AddWithValue("@albumid", this.Album.Id);
                    sqlcmd.Parameters.AddWithValue("@artistid", this.Artist.Id);
                    sqlcmd.Parameters.AddWithValue("@title", this.Title);
                    sqlcmd.Parameters.AddWithValue("@subtitle", this.Subtitle);
                    sqlcmd.Parameters.AddWithValue("@words", this.Words);
                    sqlcmd.Parameters.AddWithValue("@music", this.Music);
                    sqlcmd.Parameters.AddWithValue("@tabs", this.Tabs);
                    sqlcmd.Parameters.AddWithValue("@copyright", this.Copyright);
                    sqlcmd.Parameters.AddWithValue("@notice", this.Notice);
                    //  sqlcmd.Parameters.AddWithValue("@date_released", this.DateReleased);
                    sqlcmd.Connection.Open();
                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.Connection.Close();
                }
            }
            else
            {
                this.UpdateToDB();
            }
        }
        
        /// <summary>
        /// Deleting from database
        /// </summary>
        public void DeleteFromDB()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "DELETE FROM Compositions WHERE Id = @id";
                sqlcmd.Parameters.AddWithValue("@id", this.Id);
                sqlcmd.Connection.Open();
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Connection.Close();
            }
        }

        /// <summary>
        /// Loading of all instances from database
        /// </summary>
        /// <returns></returns>
        public static List<Composition> LoadAllFromDB()
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
                            var Composition = new Composition();
                            Composition.Id = (Guid)sqlreader[0];
                            Composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[1]);
                            Composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[2]);
                            Composition.Title = (string)sqlreader[3];
                            Composition.Subtitle = (string)sqlreader[4];
                            Composition.Words = (string)sqlreader[5];
                            Composition.Music = (string)sqlreader[6];
                            Composition.Tabs = (string)sqlreader[7];
                            Composition.Copyright = (string)sqlreader[8];
                            Composition.Notice = (string)sqlreader[9];
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