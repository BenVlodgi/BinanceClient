using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class DBProperty : Attribute
    {
        public string OverridePropertyName = null;
        public int PrimaryKeyLevel { get; set; } = -1;

        public DBProperty(string overridePropertyName = null, int primaryKeyLevel = -1)
        {
            OverridePropertyName = overridePropertyName;
            PrimaryKeyLevel = primaryKeyLevel;
        }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DBTable : Attribute
    {
        public string OverrideTableName = null;

        public DBTable(string overrideTableName = null)
        {
            OverrideTableName = overrideTableName;
        }
    }
}
