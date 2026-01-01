using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMVC_INVENTORY.Database
{
    class Transaction
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Cashier { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
