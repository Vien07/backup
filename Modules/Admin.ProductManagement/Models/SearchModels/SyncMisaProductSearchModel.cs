using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SearchModels
{
    public class SyncMisaProductSearchModel
    {
        public DateTime SyncDate { get; set; }
        public bool SyncNewProduct { get; set; }
        public bool SyncOldProduct { get; set; }
        public string? SelectMultiSyncMisa { get; set; }
    }
}
