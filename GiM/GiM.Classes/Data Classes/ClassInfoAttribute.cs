using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ClassInfoAttribute:System.Attribute
    {
        public bool MightBeSingle { get; set; }

        public ClassInfoAttribute(bool mightbesingle)
        {
            this.MightBeSingle = mightbesingle;
        }
    }
}
