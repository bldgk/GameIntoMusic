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
    [ClassInfo(true)]
    public class Track : Base
    {
        #region Properties

        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, PrimaryKey = true)]
        public Guid Id{get;set;}
        
        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = true)]
        public string FullName { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string ShortName { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = false)]
        public string Color { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Family { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Type { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string Feature { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, UniqueKey = false)]
        public Guid CompositionId
        {
            get
            {
                return Composition.Id;
            }
            set
            {
                this.CompositionId = (Guid)((Composition)Composition.Find((Guid)value)).Id;
                this.Composition = (Composition)Composition.Find(CompositionId);
            }
        }

        public List<Note> Notes { get; set; }

        public Composition Composition { get; set; }
       
        public Instrument Instrument { get; set; }

        public string NotesInTrack
        {
            get
            {
                string notes = "";
                try
                {

                    foreach (Note Note in Notes)
                    {
                        notes += Note.Name + '|';
                    }
                    notes = notes.Remove(notes.Length - 1);
                    return notes;
                }
                catch { }
                return notes;
            }
        }
        #endregion

        #region Constructors
        public Track(Composition composition, string fullname, string shortname, InstrumentFamily family, InstrumentType type, InstrumentFeature feature, string color)
        {
            this.Id = Guid.NewGuid();
            this.Composition = composition;
            this.FullName = fullname;
            this.ShortName = shortname;
            this.Family = family.ToString();
            this.Type = type.ToString();
            this.Feature = feature.ToString();
            this.Instrument = new Instrument(family, type, feature);
            this.Color = color;
            this.Notes = new List<Note>();
            this.Save();
        }

        public Track()
        {
            this.Composition = null;
            this.FullName = "";
            this.ShortName = "";
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
        /// Search by name
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Track Find(String fullname)
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
                        Track.Instrument = new Instrument((InstrumentFamily)Enum.Parse(typeof(InstrumentFamily), (string)sqlreader[4]),(InstrumentType)Enum.Parse(typeof(InstrumentType), (string)sqlreader[5]),(InstrumentFeature)Enum.Parse(typeof(InstrumentFeature), (string)sqlreader[6]));
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
        public static Track Find(Guid id)
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
                    }
                    sqlcmd.Connection.Close();
                    return Track;
                }
            }
        }

        /// <summary>
        /// Loading of all instances from database
        /// </summary>
        /// <returns></returns>
        public static List<Track> Load()
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
