using Admin.ProductManagement.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Helpers
{
    public class MisaApiTrackerHelper : IMisaApiTrackerHelper
    {
        ProductManagementContext _db;
        public MisaApiTrackerHelper(ProductManagementContext db)
        {
            _db = db;
        }

        public void SaveLog(Database.MisaApiTracker model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.MisaApiTrackers.Add(model);
                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
