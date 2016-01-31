using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using GiM.Classes.Data_Classes;
using System.Data.SqlClient;
namespace GiM.Classes
{
    public class User :Base
    {
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Composition> Compositions { get; set; }

        public ICollection<Composition> Composition()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<Composition> CompositionsOfUser = new List<Composition>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = @"
                            SELECT * 
                            FROM Compositions
                            WHERE UserId = @userid";
                sqlcmd.Parameters.AddWithValue("@userid", this.Id);
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            Composition composition = new Composition();
                            composition.Id = (Guid)sqlreader[0];
                            //composition.User = (User)User.FindThisInstanceInDB((Guid)sqlreader[1]);       sqlreader[i+1];
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
                            CompositionsOfUser.Add(composition);
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
            this.FirstName = firstname;
            this.LastName = lastname;
            this.UserName = username;
            this.Email = email;
            this.Password = password;
            this.SaveToDB();
        }



        public void SaveToDB() { }
    }
}
