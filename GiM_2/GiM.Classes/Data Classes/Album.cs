using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    [Serializable]
    public class Album : Base
    {
        #region Properties

        public string Title { get; set; }
        public DateTime DateReleased { get; set; }
        public Artist Artist { get; set; }
        public ICollection<Composition> CompositionsInAlbum()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Composition> CompositionsOfAlbum = new List<Composition>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = @"
                            SELECT * 
                            FROM Compositions
                            WHERE AlbumId = @albumid";
                sqlcmd.Parameters.AddWithValue("@albumid", this.Id);
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

        #endregion

        #region Constructors

        public Album()
        {
            this.Title = "";
            this.DateReleased = DateTime.Now;
            this.Artist = null;
            //ArtistsInAlbum = new List<Artist>();
            //  CompositionsInAlbum = new List<Composition>();
        }
        public Album(Artist artist, string title, DateTime daterelease)
        {
            this.Artist = artist;
            this.Title = title;
            this.DateReleased = daterelease;

            //ArtistsInAlbum = new List<Artist>();
            //  CompositionsInAlbum = new List<Composition>();
            this.SaveToDB();
        }
        public Album(string title, Artist artist)
        {
            this.Artist = artist;
            this.Title = title;

            //ArtistsInAlbum = new List<Artist>();
            //   CompositionsInAlbum = new List<Composition>();
            this.SaveToDB();
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Artist.LastName + ' ' + Artist.FirstName + '.' + Title;
        }

        /// <summary>
        ///  Existing of such instance in database
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
        /// Search by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Album FindThisInstanceInDB(String title)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Album Album = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Albums WHERE Title=@title";
                sqlcmd.Parameters.AddWithValue("@title", title);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Album = new Album();
                        Album.Id = (Guid)sqlreader[0];
                        Album.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[1]);
                        Album.Title = (string)sqlreader[2];

                        // Album.DateReleased = (DateTime)sqlreader[2];
                    }
                    sqlcmd.Connection.Close();
                    return Album;
                }
            }
        }

        /// <summary>
        /// Search by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Album FindThisInstanceInDB(Guid id)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Album Album = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Albums WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {

                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Album = new Album();
                        Album.Id = (Guid)sqlreader[0];
                        Album.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[1]);
                        Album.Title = (string)sqlreader[2];

                        //Album.DateReleased = (DateTime)sqlreader[2];

                    }
                    sqlcmd.Connection.Close();
                    return Album;
                }
            }
        }

        /*    public static Album FindThisInstanceInDB(DateTime date_released)
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    Album Album = null;
                    sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                    sqlcmd.CommandText = "SELECT * FROM Albums WHERE DateReleased=@date_released";
                    sqlcmd.Parameters.AddWithValue("@date_released", date_released);
                    sqlcmd.Connection.Open();
                    using (var sqlreader = sqlcmd.ExecuteReader())
                    {

                        if (sqlreader.HasRows)
                        {
                            sqlreader.Read();
                            Album = new Album();
                            Album.Id = (Guid)sqlreader[0];
                            Album.Title = (string)sqlreader[1];
                            Album.DateReleased = (DateTime)sqlreader[2];

                        }
                        sqlcmd.Connection.Close();
                        return Album;
                    }
                }*/
        //  }

        /// <summary>
        /// Search by Id
        /// </summary>
        /// <returns></returns>
        public bool DBContains()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                bool contains = false;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Albums WHERE Id=@id";
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
                sqlcmd.CommandText = "UPDATE Albums SET ArtistId = '@artistid', Title = '@title' WHERE Id=@id";
                //   sqlcmd.CommandText = "UPDATE Albums SET Title = '@title' , DateReleased = '@date_released' WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", this.Id);
                sqlcmd.Parameters.AddWithValue("@artistis", this.Artist.Id);
                sqlcmd.Parameters.AddWithValue("@title", this.Title);

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

                    sqlcmd.CommandText = "INSERT INTO Albums(Id, ArtistId, Title) values (@id,  @artistid, @title)";
                    //    sqlcmd.CommandText = "INSERT INTO Albums(Id, Title, DateReleased) values (@id, @title, @date_released)";
                    sqlcmd.Parameters.AddWithValue("@id", this.Id);
                    sqlcmd.Parameters.AddWithValue("@artistid", this.Artist.Id);
                    sqlcmd.Parameters.AddWithValue("@title", this.Title);


                    // sqlcmd.Parameters.AddWithValue("@date_released", this.DateReleased.ToString());
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
                sqlcmd.CommandText = "DELETE FROM Albums WHERE Id = @id";
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
        public static List<Album> LoadAllFromDB()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Album> ListAlbums = new List<Album>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Albums";
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            var album = new Album();
                            album.Id = (Guid)sqlreader[0];
                            album.Artist = (Artist)Artist.FindThisInstanceInDB((Guid)sqlreader[1]);
                            album.Title = (string)sqlreader[2];

                            //album.DateReleased = Convert.ToDateTime((string)sqlreader[2]);
                            ListAlbums.Add(album);

                        }
                    }
                    sqlcmd.Connection.Close();
                    return ListAlbums;
                }
            }
        }

        #endregion
    }
}