using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using GiM.Classes.Data_Classes;
using System.Data.SqlClient;
using System.Xml;
using System.Runtime.Serialization;
namespace GiM.Classes
{
    [DataContract]
    [ClassInfo(true)]
    public class User : Base
    {
        [DataMember]
        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, PrimaryKey = true)]
        public Guid Id { get; set; }

        [DataMember]
        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = true, UniqueKey = false)]
        public string FirstName { get; set; }

        [DataMember]
        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = false)]
        public string LastName { get; set; }

        [DataMember]
        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = false)]
        public string UserName { get; set; }

        [DataMember]
        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = false)]
        public string Password { get; set; }

        [DataMember]
        [PropertyInfoAttribute(System.Data.SqlDbType.NVarChar, IsNull = false, UniqueKey = true)]
        public string Email { get; set; }

        public static List<User> LocalUsers = new List<User>();

        public  ICollection<Composition> AddedCompositions()
        {
            List<Composition> Added = new List<Composition>();
            foreach (var UserComp in UserComposition.Load())
            {
                if (UserComp.UserId == this.Id)
                {
                    Added.Add(UserComp.Composition);
                }
                
            }
            return Added;
        }

        public ICollection<Composition> Compositions()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Composition> CompositionsOfUser = new List<Composition>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = @"
                            SELECT * 
                            FROM Compositions
                            WHERE AuthorId = @userid";
                sqlcmd.Parameters.AddWithValue("@userid", this.Id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            Composition Composition = new Composition();
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
                            CompositionsOfUser.Add(Composition);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return CompositionsOfUser;
                }
            }
        }

        public User()
        {
            this.FirstName = "";
            this.LastName = "";
            this.UserName = "";
            this.Email = "";
            this.Password = "";
        }


        public User(string firstname, string lastname, string username, string email, string password)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstname;
            this.LastName = lastname;
            this.UserName = username;
            this.Email = email;
            this.Password = password;
            this.Save();
            LocalUsers.Add(this);
        }

        public override string ToString()
        {
            return  UserName;
        }



        public static User Find(string email)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                User User = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
                sqlcmd.Parameters.AddWithValue("@email", email);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        User = new User();

                        User.Id = (Guid)sqlreader[0];
                        User.FirstName = (string)sqlreader[1];
                        User.LastName = (string)sqlreader[2];
                        User.UserName = (string)sqlreader[3];
                        User.Password = (string)sqlreader[4];
                        User.Email = (string)sqlreader[5];
                    }
                }
                sqlcmd.Connection.Close();
                return User;
            }
        }


        public static User Find(Guid id)
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                User User = null;
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Users WHERE Id = @id";
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        sqlreader.Read();
                        User = new User();

                        User.Id = (Guid)sqlreader[0];
                        User.FirstName = (string)sqlreader[1];
                        User.LastName = (string)sqlreader[2];
                        User.UserName = (string)sqlreader[3];
                        User.Password = (string)sqlreader[4];
                        User.Email = (string)sqlreader[5];
                    }
                }
                sqlcmd.Connection.Close();
                return User;
            }
        }

        public void LocalSave()
        {
            try
            {
                DataContractSerializer xmls = new DataContractSerializer(typeof(List<User>));
                XmlWriter xmlw = XmlWriter.Create("Profiles.xml");
                xmls.WriteObject(xmlw, LocalUsers);
                xmlw.Close();
            }
            catch { }
        }

        public void LocalLoad()
        {
            try
            {
                DataContractSerializer xmls = new DataContractSerializer(typeof(List<User>));
                XmlReader xmlr = XmlReader.Create("Profiles.xml");
                LocalUsers = (List<User>)xmls.ReadObject(xmlr);
                xmlr.Close();
            }
            catch { }
        }

        public static List<User> Load()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<User> ListUsers = new List<User>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM Users";
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            var User = new User();
                            User.Id = (Guid)sqlreader[0];
                            User.FirstName = (string)sqlreader[1];
                            User.LastName = (string)sqlreader[2];
                            User.UserName = (string)sqlreader[3];
                            User.Password = (string)sqlreader[4];
                            User.Email = (string)sqlreader[5];
                            ListUsers.Add(User);
                        }
                    }
                    sqlcmd.Connection.Close();
                    return ListUsers;
                }
            }
        }
    }
}