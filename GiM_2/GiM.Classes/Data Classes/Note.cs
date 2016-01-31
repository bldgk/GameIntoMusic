using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    public class Note
    {
        [Key]
        public string Name { get; set; }
        public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }
    }
}
