using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;

 
namespace GiM.Classes.Data_Classes
{
    [DataContract]
    public class Base
    {
        
        /// <summary>
        /// Connection string to database
        /// </summary>
        public static string ConnectionString = @"Data Source=KIRILL_PC\SQLEXPRESS;Initial Catalog=GiM.DataBaseTest;Integrated Security=True";

        //public static String ConnectionString = @"Data Source=KIRILL_PC\SQLEXPRESS;Initial Catalog=GiM.DateBase;Integrated Security=True";
        //public Guid Id { get; set; }
        public Base() { }
        
        /// <summary>
        /// Creating a table in database
        /// </summary>
        /// <returns></returns>
        public bool CreateTable()
        {
            if (!TableExistance())
            {
                SqlConnection Connection = new SqlConnection(ConnectionString);
                try
                {
                    Connection.Open();
                }
                catch (SqlException e)
                {
                    Console.Write(e.ToString());
                }
                PropertyInfo[] Properties = this.GetType().GetProperties();
                List<string> PropertyNames = new List<string>();
                string CommandText = "CREATE TABLE ";
                string parametrs_queryPart = " (";
                string primaryKey_queryPart = " PRIMARY KEY (";
                foreach (PropertyInfo Property in Properties)
                {
                    PropertyInfoAttribute PropertyInfo = (PropertyInfoAttribute)Attribute.GetCustomAttribute(Property, typeof(PropertyInfoAttribute));
                    if (PropertyInfo != null)
                    {
                        if (PropertyInfo.DataType == System.Data.SqlDbType.NVarChar)
                        {
                            parametrs_queryPart += (Property.Name + ' ' + PropertyInfo.DataTypeToString); //+' ' 
                        }
                        else
                        {
                            parametrs_queryPart += (Property.Name + ' ' + PropertyInfo.DataType);
                        }
                        if (!PropertyInfo.IsNull)
                            parametrs_queryPart += " not null, ";
                        else
                            parametrs_queryPart += ", ";
                        if (PropertyInfo.PrimaryKey)
                            primaryKey_queryPart += Property.Name + ")";
                    }
                    else
                    {
                        ////if (Property.PropertyType.Name == "String")
                        ////    parametrs_queryPart += "text, ";
                        //if (Property.PropertyType.Name == "Guid")
                        //{
                        //    parametrs_queryPart += "Uniqueidentifier";
                        //    if (Property.Name == "Id")
                        //        parametrs_queryPart += " not null, ";
                        //    else
                        //        parametrs_queryPart += ", ";
                        //}
                        //if (Property.PropertyType.Name == "Int")
                        //    parametrs_queryPart += "int, ";
                        //if (Property.PropertyType.Name == "Bool")
                        //    parametrs_queryPart += "bit not null, ";
                    }
                }
                CommandText += GetType().Name + 's' + parametrs_queryPart.Remove(parametrs_queryPart.Length - 2) + primaryKey_queryPart + " )";
                SqlCommand Command = new SqlCommand(CommandText, Connection);
                try
                {
                    Command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException e)
                {
                    Console.Write(e.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Table existanse
        /// </summary>
        /// <returns></returns>
        public bool TableExistance()
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand("SELECT COUNT(*) FROM " + GetType().Name + 's', Connection);
            try
            {
                Connection.Open();
                Command.ExecuteNonQuery();
                return true;
            }
            catch (SqlException e)
            {
                Connection.Close();
                Connection.Dispose();
                return false;
            }

        }

        public bool ObjectExistence()
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                PropertyInfo[] Properties = GetType().GetProperties();
                using (SqlCommand Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    string selectQueryPart = " WHERE ";
                    foreach (PropertyInfo Property in Properties)
                    {
                        try
                        {
                            PropertyInfoAttribute PropertyInfo = (PropertyInfoAttribute)Property.GetCustomAttribute(typeof(PropertyInfoAttribute));
                            if (PropertyInfo.UniqueKey)
                            {
                                if (Property.Name == "Id")
                                {
                                    selectQueryPart += Property.Name + " = @" + Property.Name + " && WHERE ";
                                    Command.Parameters.Add("@" + Property.Name, System.Data.SqlDbType.UniqueIdentifier).Value = Property.GetValue(this);
                                }
                                else
                                {
                                    selectQueryPart += Property.Name + " = @" + Property.Name + " && WHERE ";
                                    Command.Parameters.Add("@" + Property.Name, System.Data.SqlDbType.NVarChar).Value = Property.GetValue(this);
                                }
                            }
                        }
                        catch { }
                    }
                    Command.CommandText = "SELECT * FROM " + GetType().Name + 's' + selectQueryPart.Remove(selectQueryPart.Length - 9);
                    Connection.Open();
                    using (SqlDataReader DataReader = Command.ExecuteReader())
                    {
                        DataReader.Read();
                        if (DataReader.HasRows)
                            return true;
                        else
                            return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// Saving database
        /// </summary>
        public virtual void Save()
        {
            CreateTable();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand())
                {
                    if (ObjectExistence())
                    {
                        //MessageBox.WriteLine("Such element already exists");
                        return;
                    }
                    else
                    {
                        PropertyInfo[] Properties = GetType().GetProperties();
                        string insertQuery_part = " ( ";
                        string insertQuery_part2 = " ( ";
                        foreach (PropertyInfo Property in Properties)
                        {
                            if (Property.GetValue(this) != null)
                            {
                                try
                                {
                                    if (Attribute.IsDefined(Property, typeof(PropertyInfoAttribute)) == true)
                                    {
                                        insertQuery_part += (Property.Name + ", ");
                                        insertQuery_part2 += ("@" + Property.Name + ", ");
                                        PropertyInfoAttribute PropertyInfo = (PropertyInfoAttribute)Property.GetCustomAttribute(typeof(PropertyInfoAttribute));
                                        Command.Parameters.Add("@" + Property.Name, PropertyInfo.DataType).Value = Property.GetValue(this);
                                    }
                                }
                                catch { }
                            }
                        }
                        insertQuery_part = (insertQuery_part.Remove(insertQuery_part.Length - 2) + " ) ");
                        insertQuery_part2 = (insertQuery_part2.Remove(insertQuery_part2.Length - 2) + " ) ");
                        Command.CommandText = "INSERT INTO " + GetType().Name + 's' + insertQuery_part + "VALUES" + insertQuery_part2;
                        Command.Connection = Connection;
                        try
                        {
                            Connection.Open();
                            Command.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Console.Write(e.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updating the database
        /// </summary>
        /// <param name="Object"></param>
        public void Update(object Object)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand())
                {
                    PropertyInfo[] Properties = GetType().GetProperties();
                    string updateQuery_part = " ";
                    string updateQuery_part2 = " WHERE ";
                    foreach (PropertyInfo Property in Properties)
                    {
                        PropertyInfoAttribute PropertyInfo = (PropertyInfoAttribute)Property.GetCustomAttribute(typeof(PropertyInfoAttribute));
                        if (Attribute.IsDefined(Property, typeof(PropertyInfoAttribute)) == true)
                        {
                            if (!PropertyInfo.PrimaryKey)
                            {
                                updateQuery_part += (Property.Name + " = " + "@" + Property.Name + ", ");
                                Command.Parameters.Add("@" + Property.Name, PropertyInfo.DataType).Value = Property.GetValue(Object);
                                if (PropertyInfo.UniqueKey)
                                {
                                    updateQuery_part2 += Property.Name + " = " + "@new_" + Property.Name + ", ";
                                    Command.Parameters.Add("@new_" + Property.Name, PropertyInfo.DataType).Value = Property.GetValue(this);
                                }
                            }
                        }
                    }
                    updateQuery_part = (updateQuery_part.Remove(updateQuery_part.Length - 2));
                    updateQuery_part2 = (updateQuery_part2.Remove(updateQuery_part2.Length - 2));
                    Command.CommandText = "UPDATE " + GetType().Name +'s' + " SET " + updateQuery_part + updateQuery_part2;
                    Command.Connection = Connection;
                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.Write(e.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Deleting from database
        /// </summary>
        public void Delete()
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand("DELETE FROM " + GetType().Name +'s'+ " WHERE Id = '" + this.GetType().GetProperty("Id").GetValue(this).ToString() + "'", Connection);
            try
            {
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.Write(e.ToString());
            }
            finally
            {
                Connection.Close();
                Connection.Dispose();
            }
        }


    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Reflection;
 
//namespace GiM.Classes.Data_Classes
//{
//    [Serializable]
//    public class Base
//    {
        
//        /// <summary>
//        /// Connection string to database
//        /// </summary>
//        public static string ConnectionString = @"Data Source=KIRILL_PC\SQLEXPRESS;Initial Catalog=GiM.DataBaseTest;Integrated Security=True";

//        //public static String ConnectionString = @"Data Source=KIRILL_PC\SQLEXPRESS;Initial Catalog=GiM.DateBase;Integrated Security=True";
//        //public Guid Id { get; set; }
//        //public Base()
//        //{
//        //     Id = Guid.NewGuid();
//        //}


//        public bool CreateTable()
//        {
//            if (!TableExistance())
//            {
//                SqlConnection Connection = new SqlConnection(ConnectionString);
//                try
//                {
//                    Connection.Open();
//                }
//                catch (SqlException e)
//                {
//                    Console.Write(e.ToString());
//                }
//                FieldInfo[] Fields = this.GetType().GetFields();
//                List<string> Field_names = new List<string>();
//                string CommandText = " CREATE TABLE ";
//                string parametrs_queryPart = " (";
//                string primaryKey_queryPart = " PRIMARY KEY (";



//                foreach (FieldInfo Field in Fields)
//                {
//                    FPInfoAttribute FPAttribute = (FPInfoAttribute)Attribute.GetCustomAttribute(Field, typeof(FPInfoAttribute));
//                    if (FPAttribute != null)
//                    {
//                        parametrs_queryPart += (Field.Name + FPAttribute.DataType);
//                        if (!FPAttribute.IsNull)
//                            parametrs_queryPart += " not null, ";
//                        else
//                            parametrs_queryPart += ", ";
//                        if (FPAttribute.PrimaryKey)
//                            primaryKey_queryPart += Field.Name + ")";
//                    }
//                    else
//                    {
//                        if (Field.FieldType.Name == "String")
//                            parametrs_queryPart += "text, ";

//                        if (Field.FieldType.Name == "Guid")
//                        {
//                            parametrs_queryPart += "uniqueidentifier";
//                            if (Field.Name == "Id")
//                                parametrs_queryPart += " not null, ";
//                            else
//                                parametrs_queryPart += ", ";
//                        }
//                        if (Field.FieldType.Name == "Int")
//                            parametrs_queryPart += "int, ";
//                        if (Field.FieldType.Name == "Bool")
//                            parametrs_queryPart += "bit not null, ";
//                    }

//                }

//                CommandText += GetType().Name + parametrs_queryPart.Remove(parametrs_queryPart.Length - 2) + primaryKey_queryPart + " )";
//                SqlCommand Command = new SqlCommand(CommandText, Connection);

//                try
//                {
//                    Command.ExecuteNonQuery();
//                    return true;
//                }
//                catch (SqlException e)
//                {
//                    Console.Write(e.ToString());
//                    return false;
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Table existanse
//        /// </summary>
//        /// <returns></returns>
//        public bool TableExistance()
//        {
//            SqlConnection Connection = new SqlConnection(ConnectionString);
//            SqlCommand Command = new SqlCommand("SELECT COUNT(*) FROM " + GetType().Name, Connection);
//            try
//            {
//                Connection.Open();
//                Command.ExecuteNonQuery();
//                return true;
//            }
//            catch (SqlException e)
//            {
//                Connection.Close();
//                Connection.Dispose();
//                return false;
//            }

//        }

//        public bool ObjectExistence()
//        {
//            using (SqlConnection Connection = new SqlConnection(ConnectionString))
//            {
//                FieldInfo[] Fields = GetType().GetFields();

//                using (SqlCommand Command = new SqlCommand())
//                {
//                    Command.Connection = Connection;
//                    string selectQueryPart = " WHERE ";
//                    foreach (FieldInfo Field in Fields)
//                    {
//                        FPInfoAttribute FPAttribute = (FPInfoAttribute)Field.GetCustomAttribute(typeof(FPInfoAttribute));
//                        if (FPAttribute.UniqueKey)
//                        {
//                            selectQueryPart += Field.Name + " = @" + Field.Name + " && WHERE ";
//                            Command.Parameters.Add("@" + Field.Name, System.Data.SqlDbType.VarChar).Value = Field.GetValue(this);
//                        }
//                    }

//                    string selectQuery = "SELECT * FROM " + GetType().Name + selectQueryPart.Remove(selectQueryPart.Length - 9);
//                    Command.CommandText = selectQuery;
//                    Connection.Open();

//                    using (SqlDataReader DataReader = Command.ExecuteReader())
//                    {
//                        DataReader.Read();
//                        if (DataReader.HasRows)
//                            return true;
//                        else
//                            return false;
//                    }

//                }
//            }

//        }

//        /// <summary>
//        /// Saving database
//        /// </summary>
//        public virtual void Save()
//        {
//            CreateTable();
//            using (SqlConnection Connection = new SqlConnection(ConnectionString))
//            {
//                using (SqlCommand Command = new SqlCommand())
//                {
//                    if (ObjectExistence())
//                    {
//                        Console.WriteLine("Such element already exists");
//                        return;
//                    }
//                    else
//                    {
//                        FieldInfo[] Fields = GetType().GetFields();
//                        string insertQuery_part = " ( ";
//                        string insertQuery_part2 = " ( ";
//                        foreach (FieldInfo Field in Fields)
//                        {
//                            if (Field.GetValue(this) != null)
//                            {
//                                insertQuery_part += (Field.Name + ", ");
//                                insertQuery_part2 += ("@" + Field.Name + ", ");
//                                FPInfoAttribute atrr = (FPInfoAttribute)Field.GetCustomAttribute(typeof(FPInfoAttribute));
//                                Command.Parameters.Add("@" + Field.Name, atrr.DataType).Value = Field.GetValue(this);
//                            }
//                        }
//                        insertQuery_part = (insertQuery_part.Remove(insertQuery_part.Length - 2) + " ) ");
//                        insertQuery_part2 = (insertQuery_part2.Remove(insertQuery_part2.Length - 2) + " ) ");
//                        Command.CommandText = "INSERT INTO " + GetType().Name + insertQuery_part + "VALUES" + insertQuery_part2;
//                        Command.Connection = Connection;
//                        try
//                        {
//                            Connection.Open();
//                            Command.ExecuteNonQuery();
//                            Console.WriteLine("Object successfully saved");
//                        }
//                        catch (SqlException e)
//                        {
//                            Console.Write(e.ToString());
//                        }
//                    }

//                }
//            }

//        }

//        /// <summary>
//        /// Updating the database
//        /// </summary>
//        /// <param name="Object"></param>
//        public void Update(object Object)
//        {
//            using (SqlConnection Connection = new SqlConnection(ConnectionString))
//            {
//                using (SqlCommand Command = new SqlCommand())
//                {
//                    FieldInfo[] Fields = GetType().GetFields();
//                    string updateQuery_part = " ";
//                    string updateQuery_part2 = " WHERE ";
//                    foreach (FieldInfo Field in Fields)
//                    {
//                        FPInfoAttribute atrr = (FPInfoAttribute)Field.GetCustomAttribute(typeof(FPInfoAttribute));
//                        if (!atrr.PrimaryKey)
//                        {
//                            updateQuery_part += (Field.Name + " = " + "@" + Field.Name + ", ");
//                            Command.Parameters.Add("@" + Field.Name, atrr.DataType).Value = Field.GetValue(Object);
//                            if (atrr.UniqueKey)
//                            {
//                                updateQuery_part2 += Field.Name + " = " + "@new_" + Field.Name + ", ";
//                                Command.Parameters.Add("@new_" + Field.Name, atrr.DataType).Value = Field.GetValue(this);
//                            }
//                        }
//                    }
//                    updateQuery_part = (updateQuery_part.Remove(updateQuery_part.Length - 2));
//                    updateQuery_part2 = (updateQuery_part2.Remove(updateQuery_part2.Length - 2));
//                    Command.CommandText = "UPDATE " + GetType().Name + " SET " + updateQuery_part + updateQuery_part2;
//                    Command.Connection = Connection;
//                    try
//                    {
//                        Connection.Open();
//                        Command.ExecuteNonQuery();
//                        Console.WriteLine("Object successfully updated");
//                    }
//                    catch (SqlException e)
//                    {
//                        Console.Write(e.ToString());
//                    }

//                }
//            }

//        }

//        /// <summary>
//        /// Deleting from database
//        /// </summary>
//        public void Delete()
//        {
//            SqlConnection Connection = new SqlConnection(ConnectionString);

//            SqlCommand Command = new SqlCommand("DELETE FROM " + GetType().Name + " WHERE Id = '" + this.GetType().GetField("Id").GetValue(this).ToString() + "'", Connection);
//            try
//            {
//                Connection.Open();
//                Command.ExecuteNonQuery();
//            }
//            catch (SqlException e)
//            {
//                Console.Write(e.ToString());
//            }
//            finally
//            {
//                Connection.Close();
//                Connection.Dispose();
//            }
//        }


        
//    }
//}
