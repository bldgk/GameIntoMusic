using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GiM.Classes.Data_Classes;


namespace GiM.Classes.Programm_Classes
{
    public class Controller
    {

        public void CreateArtist()
        {
            Artist Artist = new Artist();
        }

        public void CreateArtist(User user, string lastname, string firstname)
        {}

        public void CreateArtist(string lastname, string firstname)
        {
            Artist Artist = new Artist(lastname, firstname);
        }

    }
}
