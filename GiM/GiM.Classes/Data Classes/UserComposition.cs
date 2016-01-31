using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GiM.Classes.Data_Classes;

namespace GiM.Classes.Data_Classes
{
    [ClassInfo(true)]
    public class UserComposition:Base
    {
        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, PrimaryKey = true, UniqueKey=true)]
        public Guid Id { get; set; }

        [PropertyInfoAttribute(System.Data.SqlDbType.UniqueIdentifier, IsNull = false, UniqueKey = false)]
        public Guid UserId 
        {
            get
            {
                return User.Id;
            }
            set
            {

                this.UserId = (Guid)((User)User.Find((Guid)value)).Id;
                this.User = (User)User.Find(this.UserId);
            }

        }

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
                this.Composition = (Composition)Composition.Find(this.CompositionId);
            }
        }
            


        public User User { get; set; }


        public Composition Composition { get; set; }

        public UserComposition()
        {
            this.Id = Guid.NewGuid();
            this.User = null;
            this.Composition = null;
        }

        public UserComposition(User user, Composition composition)
        {
            this.Id = Guid.NewGuid();
            this.User = user;
            this.Composition = composition;
            this.Save();
        }


        public static List<UserComposition> Load()
        {
            using (SqlCommand sqlcmd = new SqlCommand())
            {
                List<UserComposition> ListUserCompositions = new List<UserComposition>();
                sqlcmd.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                sqlcmd.CommandText = "SELECT * FROM UserCompositions";
                sqlcmd.Connection.Open();
                using (var sqlreader = sqlcmd.ExecuteReader())
                {
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            UserComposition UserComposition = new UserComposition();
                            UserComposition.Id = (Guid)sqlreader[0];
                            UserComposition.User = (User)User.Find((Guid)sqlreader[1]);
                            UserComposition.Composition = (Composition)Composition.Find((Guid)sqlreader[2]);

                            ListUserCompositions.Add(UserComposition);

                        }
                    }
                    sqlcmd.Connection.Close();
                    return ListUserCompositions;
                }
            }
        }

    }
}
