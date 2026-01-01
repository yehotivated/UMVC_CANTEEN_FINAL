using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMVC_INVENTORY.Database
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int StockQty { get; set; }
        public int CriticalLevel { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }

        public int CategoryId { get; set; }
    }
}