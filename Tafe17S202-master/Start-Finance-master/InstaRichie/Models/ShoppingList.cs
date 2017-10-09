using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    class ShoppingList
    {
        [PrimaryKey, AutoIncrement]
        public string ID { get; set; }

        [NotNull]
        public string ShoppingDate{ get; set; }
        
        [NotNull]
        public string NameOfItem { get; set; }

        [NotNull]
        public string PriceQuoted { get; set; }

    }
}
