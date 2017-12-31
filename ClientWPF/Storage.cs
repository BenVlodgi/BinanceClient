using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Binance.Net;
using Binance.Net.Objects;

namespace Binance.Net.ClientWPF
{
    public class Storage
    {
        string binanceLocation = "BinanceHistory.sqlite";
        public SQLiteConnection DBConnection { get; protected set; }


        Dictionary<Type, Dictionary<string, Type>> tableMap = new Dictionary<Type, Dictionary<string, Type>>();

        public static string GetSQLiteType(Type type)
        {
            var @switch = new Dictionary<Type, Func<string>>
            {
                { typeof(BinanceKline), () => "" },
                { typeof(string), () => $"varchar(64)" },
                { typeof(decimal), () => $"decimal(20,10)" },
            };
            return @switch.ContainsKey(type)
                ? @switch[type]()
                : type.Name;
        }

        public Storage()
        {
            List<string> rebuildTables = new List<string>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var dbTables = assembly.GetTypesWithAttribute(typeof(DBTable));
                foreach (var table in dbTables)
                {
                    if (!tableMap.ContainsKey(table))
                        tableMap.Add(table, null);
                }
            }

            foreach (var key in tableMap.Keys.ToList())
            {
                tableMap[key] = key.GetProperties().ToDictionary(pi => pi.Name, pi => pi.PropertyType);
                //Recreate Tables
                string props = string.Join(",", tableMap[key].Select(kvp => $"{kvp.Key} {GetSQLiteType(kvp.Value)}"));
                string sql = $"create table if not exists {key.Name} ({props})";
                //File.AppendAllText("sql.txt", sql + "\n");
                rebuildTables.Add(sql);
            }

            // Make sure DB exists
            if (!File.Exists(binanceLocation))
            {
                SQLiteConnection.CreateFile(binanceLocation);
            }

            DBConnection = new SQLiteConnection($"Data Source={binanceLocation};Version=3;");
            DBConnection.Open();

            foreach (var sql in rebuildTables)
            {
                SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
                command.ExecuteNonQueryAsync();
            }

            DBConnection.Close();
        }
    }
}
