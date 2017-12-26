using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF
{
    class StorageAttributes
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class StorageProperty : Attribute
    {
        public int PrimaryKeyLevel { get; set; } = -1;

        public StorageProperty(int primaryKeyLevel = -1)
        {
            PrimaryKeyLevel = primaryKeyLevel;
        }
    }
}
