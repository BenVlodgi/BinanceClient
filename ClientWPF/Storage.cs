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
        SQLiteConnection _dbConnection;

        Dictionary<Type, Dictionary<string, Type>> tableMap = new Dictionary<Type, Dictionary<string, Type>>()
        {
            { typeof(BinanceKline), null },
        };

        private string GetSQLiteType(Type type)
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
            foreach (var key in tableMap.Keys.ToList())
            {
                tableMap[key] = key.GetProperties().ToDictionary(pi => pi.Name, pi => pi.PropertyType);
            }

            List<string> rebuildTables = new List<string>();
            // Make sure DB exists
            if (!File.Exists(binanceLocation))
            {
                SQLiteConnection.CreateFile(binanceLocation);

                foreach (var key in tableMap.Keys)
                {
                    //Recreate Tables
                    string props = string.Join(",", tableMap[key].Select(kvp => $"{kvp.Key} {GetSQLiteType(kvp.Value)}"));
                    string sql = $"create table {key.Name} ({props})";
                    //File.AppendAllText("sql.txt", sql + "\n");
                    rebuildTables.Add(sql);
                }
            }

            _dbConnection = new SQLiteConnection($"Data Source={binanceLocation};Version=3;");
            _dbConnection.Open();

            foreach(var sql in rebuildTables)
            {
                SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
                command.ExecuteNonQueryAsync();
            }

            _dbConnection.Close();
        }
    }
}
