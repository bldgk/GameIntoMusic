using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlTypes;

namespace GiM.Classes.Data_Classes
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyInfoAttribute:System.Attribute
    {
        public bool PrimaryKey;
        public bool UniqueKey;
        public bool SecondaryKey;
        public bool IsNull;
        public System.Data.SqlDbType DataType;
        public string DataTypeToString
        {
            get
            {
               // DataType == System.Data.SqlDbType.
                if (DataType == System.Data.SqlDbType.NVarChar)
                    return " " + DataType.ToString() + "(MAX) ";
                return " " + DataType.ToString() + " ";
            }
        }

        //public FPInfoAttribute(string dataType)
        public PropertyInfoAttribute(System.Data.SqlDbType dataType)
        {
            DataType = dataType;
            PrimaryKey = false;// Default value.
            UniqueKey = false;
            SecondaryKey = false;
            IsNull = true;
        }
    }
}
