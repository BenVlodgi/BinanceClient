using Binance.Net.ClientWPF.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF
{
    public class StorageTableBase<TableType>
    {
        public SQLiteConnection Connection { get; set; }
        public static string TableName { get; protected set; }
        protected static Dictionary<Type, Dictionary<PropertyInfo, string>> propertyNames { get; set; } = new Dictionary<Type, Dictionary<PropertyInfo, string>>();

        public StorageTableBase()
        {
            TableName = typeof(TableType).Name;

            var classDBAttribute = typeof(TableType).GetCustomAttributes(typeof(DBTable), true).FirstOrDefault() as DBTable;
            if (classDBAttribute != null && classDBAttribute.OverrideTableName != null)
            {
                TableName = classDBAttribute.OverrideTableName;
            }
        }

        public bool Save()
        {
            if (Connection == null)
                return false;

            bool connectionOpen = false;
            try
            {
                Connection.Open();
                connectionOpen = true;
                var sql = GetInsertUpdateString();
                var command = new SQLiteCommand(sql, Connection);
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.ShowMessage($"Saving SQLITE data failed", "SQLITE save fail", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                if (connectionOpen)
                    Connection.Close();
            }
            return true;
        }

        public static string GetTableCreateSQL()
        {
            Type type = typeof(TableType);
            if (!propertyNames.ContainsKey(type))
                GenerateTableProperties(type);

            string props = string.Join(",", propertyNames[type].Select(kvp => $"{kvp.Value} {Storage.GetSQLiteType(kvp.Key.PropertyType)}"));
            return $"create table {TableName} ({props})";
        }

        protected string GetInsertUpdateString()
        {
            Type type = typeof(TableType);
            if (!propertyNames.ContainsKey(type))
                GenerateTableProperties(type);

            List<string> headers = new List<string>();
            List<string> values = new List<string>();
            foreach (var prop in propertyNames[type])
            {
                headers.Add(prop.Value);
                values.Add(prop.Key.GetValue(this).ToString());
            }
            var sqlHeaders = string.Join(", ", headers);
            var sqlValues = string.Join(", ", values.Select(value => $"{'"'}{value}{'"'}"));
            var sql = $"replace into {TableName}({sqlHeaders}) values ({sqlValues});";

            return sql;
        }


        protected static void GenerateTableProperties(Type type)
        {
            if (propertyNames.ContainsKey(type))
                return;

            var dict = new Dictionary<PropertyInfo, string>();
            var obj = Activator.CreateInstance(type);


            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    var dbProp = attr as DBProperty;
                    if (dbProp != null)
                    {
                        dict.Add(prop, string.IsNullOrWhiteSpace(dbProp.OverridePropertyName) ? prop.Name : dbProp.OverridePropertyName);
                    }
                }
            }
            propertyNames.Add(type, dict);
        }
    }
}
