using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GiM.Classes.Programm_Classes;
using GiM.Classes.Data_Classes;

namespace GiM.Classes
{
    [Serializable]
    public class Track : Base
    {
        #region Properties

        public string Name { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Color { get; set; }
        public Composition Composition { get; set; }
        public InstrumentFamily Family { get; set; }
        public InstrumentType Type { get; set; }
        public InstrumentFeature Feature { get; set; }
        public Instrument Instrument { get; set; }
        public virtual ICollection<Note> NotesInTrack { get; set; }

        #endregion

        #region Constructors
        public Track(Composition composition, string fullname, string shortname, InstrumentFamily family, InstrumentType type, InstrumentFeature feature, string color)
        {
            this.Composition = composition;
            this.FullName = fullname;
            this.ShortName = shortname;
            this.Instrument = new Instrument(family, type, feature);
            this.Family = family;
            this.Type = type;
            this.Feature = feature;
            this.Color = color;
            this.SaveToDB();
        }

        public Track()
        {
            this.Composition = null;
            this.FullName = "";
            this.ShortName = "";
            this.Instrument = null;
            this.Color = "";
        }

        #endregion


        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ShortName;

        }

        /// <summary>
        /// Existing of such instance in database
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool ExistingOfThisInstance(string fullname)
        {
            if (FindThisInstanceInDB(fullname) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Search by name
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Track FindThisInstanceInDB(String fullname)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Track Track = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Tracks WHERE FullName=@fullname";
                sqlcmd.Parameters.AddWithValue("@fullname", fullname);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Track = new Track();
                        Track.Id = (Guid)sqlreader[0];
                        Track.Composition = (Composition)Composition.FindThisInstanceInDB((Guid)sqlreader[1]);
                        Track.FullName = (string)sqlreader[2];
                        Track.ShortName = (string)sqlreader[3];
                        Track.Family = (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]);
                        Track.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]);
                        Track.Feature = (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[5]);
                        Track.Color = (string)sqlreader[7];
                    }
                    sqlcmd.Connection.Close();
                    return Track;
                }
            }
        }

        /// <summary>
        /// Search by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Track FindThisInstanceInDB(Guid id)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                Track Track = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Tracks WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        Track = new Track();
                        Track.Id = (Guid)sqlreader[0];
                        Track.Composition = (Composition)Composition.FindThisInstanceInDB((Guid)sqlreader[1]);
                        Track.FullName = (string)sqlreader[2];
                        Track.ShortName = (string)sqlreader[3];
                        Track.Family = (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]);
                        Track.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]);
                        Track.Feature = (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[5]);
                        Track.Color = (string)sqlreader[7];
                    }
                    sqlcmd.Connection.Close();
                    return Track;
                }
            }
        }

        /// <summary>
        /// Db containing
        /// </summary>
        /// <returns></returns>
        public bool DBContains()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                bool contains = false;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Tracks WHERE Id=@id";
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
                sqlcmd.CommandText = "UPDATE Tracks SET CompositionId = '@compositionid', FullName = '@fullname', ShortName = '@shortname', Family = '@family', Type = '@type', Feature = '@feature', Color = '@color' WHERE Id=@id";
                sqlcmd.Parameters.AddWithValue("@id", this.Id);
                sqlcmd.Parameters.AddWithValue("@compositionid", this.Composition.Id);
                sqlcmd.Parameters.AddWithValue("@fullname", this.FullName);
                sqlcmd.Parameters.AddWithValue("@shortname", this.ShortName);
                sqlcmd.Parameters.AddWithValue("@family", this.Family.ToString());
                sqlcmd.Parameters.AddWithValue("@type", this.Type.ToString());
                sqlcmd.Parameters.AddWithValue("@feature", this.Feature.ToString());
                sqlcmd.Parameters.AddWithValue("@color", this.Color);
                sqlcmd.Connection.Open();
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Connection.Close();
            }
        }

        /// <summary>
        /// Saving the instance to database
        /// </summary>
        public void SaveToDB()
        {
            if (!this.DBContains())
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);

                    sqlcmd.CommandText = "INSERT INTO Tracks(Id, CompositionId, FullName, ShortName, Family, Type, Feature, Color) values (@id, @compositionid, @fullname, @shortname, @family, @type, @feature, @color)";
                    sqlcmd.Parameters.AddWithValue("@id", this.Id);
                    sqlcmd.Parameters.AddWithValue("@compositionid", this.Composition.Id);
                    sqlcmd.Parameters.AddWithValue("@fullname", this.FullName);
                    sqlcmd.Parameters.AddWithValue("@shortname", this.ShortName);
                    sqlcmd.Parameters.AddWithValue("@family", this.Family.ToString());
                    sqlcmd.Parameters.AddWithValue("@type", this.Type.ToString());
                    sqlcmd.Parameters.AddWithValue("@feature", this.Feature.ToString());
                    sqlcmd.Parameters.AddWithValue("@color", this.Color);
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
                sqlcmd.CommandText = "DELETE FROM Tracks WHERE Id = @id";
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
        public static List<Track> LoadAllFromDB()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Track> ListTracks = new List<Track>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Tracks";
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            var Track = new Track();
                            Track.Id = (Guid)sqlreader[0];
                            Track.Id = (Guid)sqlreader[0];
                            Track.Composition = (Composition)Composition.FindThisInstanceInDB((Guid)sqlreader[1]);
                            Track.FullName = (string)sqlreader[2];
                            Track.ShortName = (string)sqlreader[3];
                            Track.Family = (InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]);
                            Track.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]);
                            Track.Feature = (InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[6]);
                            Track.Color = (string)sqlreader[7];
                            ListTracks.Add(Track);

                        }
                    }
                    sqlcmd.Connection.Close();
                    return ListTracks;
                }
            }
        #endregion
        }
    }
}
