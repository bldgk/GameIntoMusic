using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    [Serializable]
    public class Artist : Base
    {
        #region Properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return LastName + ' ' + FirstName; } }
        public virtual ICollection<Composition> CompositionsOfArtist() 
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Composition> CompositionsOfAlbum = new List<Composition>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = @"
                            SELECT * 
                            FROM Compositions
                            WHERE ArtistId = @artistid";
                sqlcmd.Parameters.AddWithValue("@artistid", this.Id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            Composition composition = new Composition();
                            composition.Id = (Guid)sqlreader[0];
                            composition.Album = (Album)Album.FindThisInstanceInDB((Guid)sqlreader[1]);
                            composition.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[2]);
                            composition.Title = (string)sqlreader[3];
                            composition.Subtitle = (string)sqlreader[4];
                            composition.Words = (string)sqlreader[5];
                            composition.Music = (string)sqlreader[6];
                            composition.Tabs = (string)sqlreader[7];
                            composition.Copyright = (string)sqlreader[8];
                            composition.Notice = (string)sqlreader[9];
                            //Composition.DateCreated = (DateTime)sqlreader[9];
                            CompositionsOfAlbum.Add(composition);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return CompositionsOfAlbum;
                }
            }
        }
        public List<Album> AlbumsOfArtist()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Album> ArtistsInAlbum = new List<Album>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = @"SELECT * FROM Albums WHERE ArtistId = @artistid";
                sqlcmd.Parameters.AddWithValue("@artistid", this.Id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            Album album = new Album();
                            album.Id = (Guid)sqlreader[0];
                            album.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[1]);
                            album.Title = (string)sqlreader[2];
                            ArtistsInAlbum.Add(album);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return ArtistsInAlbum;
                }
            }
        }

        #endregion

        #region Constructors

        public Artist()
        {
            this.FirstName = "";
            this.LastName = "";
        }
        
        public Artist(string lastname,string firstname)
        {
           // this.Id = Guid.NewGuid();
            this.FirstName = firstname;
            this.LastName = lastname;
            this.SaveToDB();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Artist ToString();
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Existing of such instance in database
        /// </summary>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public static bool ExistingOfThisInstance(string lastname)
        {
            if (FindThisInstanceInDB(lastname) != null)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Search by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Artist FindThisInstanceInDB(String name)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Artist Artist = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Artists WHERE LastName=@lastname";
                sqlcmd.Parameters.AddWithValue("@lastname", name);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Artist = new Artist();
                        Artist.Id = (Guid)sqlreader[0];
                        Artist.LastName = (string)sqlreader[1];
                        Artist.FirstName = (string)sqlreader[2];
                        
                    }
                    sqlcmd.Connection.Close();
                    return Artist;
                }
            }
        }
        
        /// <summary>
        /// Search by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Artist FindThisInstanceInDB(Guid id)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Artist Artist = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Artists WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {

                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Artist = new Artist();
                        Artist.Id = (Guid)sqlreader[0];
                        Artist.LastName = (string)sqlreader[1];
                        Artist.FirstName = (string)sqlreader[2];

                    }
                    sqlcmd.Connection.Close();
                    return Artist;
                }
            }
        }

        /// <summary>
        /// If database contains such instance
        /// </summary>
        /// <returns></returns>
        public bool DBContains()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                bool contains = false;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Artists WHERE Id=@id";
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
                sqlcmd.CommandText = "UPDATE Artists SET LastName = '@lastname' , FirstName = '@firstname' WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", this.Id);
                sqlcmd.Parameters.AddWithValue("@lastname", this.LastName);
                sqlcmd.Parameters.AddWithValue("@firstname", this.FirstName);
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
                    sqlcmd.CommandText = "INSERT INTO Artists(Id, LastName, FirstName) values (@id, @lastname, @firstname)";
                    sqlcmd.Parameters.AddWithValue("@id", this.Id);
                    sqlcmd.Parameters.AddWithValue("@lastname", this.LastName);
                    sqlcmd.Parameters.AddWithValue("@firstname", this.FirstName);
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
                sqlcmd.CommandText = "DELETE FROM Artists WHERE Id = @id";
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
        public static List<Artist> LoadAllFromDB()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Artist> ListArtists = new List<Artist>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Artists";
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            var artist = new Artist();
                            artist.Id = (Guid)sqlreader[0];
                            artist.LastName = (string)sqlreader[1];
                            artist.FirstName = (string)sqlreader[2];
                            ListArtists.Add(artist);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return ListArtists;
                }
            }
        }

        #endregion
    }
}
      /*
        #region Properties
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Composition> CompositionsOfArtist { get; set; }
        public virtual ICollection<Album> ArtistInAlbum { get; set; }
        #endregion

        #region Constructors
        public Artist()
        {
            this.FirstName = "";
            this.LastName = "";
            CompositionsOfArtist = new List<Composition>();
            ArtistInAlbum = new List<Album>();
        }
        public Artist(string firstname, string lastname)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstname;
            this.LastName = lastname;
            CompositionsOfArtist = new List<Composition>();
            ArtistInAlbum = new List<Album>();
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return FirstName+LastName;
        }
        #endregion
       * */
 