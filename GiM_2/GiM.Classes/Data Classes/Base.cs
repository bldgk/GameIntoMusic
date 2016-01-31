using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    [Serializable]
    public class Base
    {
        public Guid Id { get; set; }
        public static String ConnectionString = @"Data Source=KIRILL_PC\SQLEXPRESS;Initial Catalog=GiM.DateBase;Integrated Security=True";
        public Base()
        {
            Id = Guid.NewGuid();
        }

    }
}
