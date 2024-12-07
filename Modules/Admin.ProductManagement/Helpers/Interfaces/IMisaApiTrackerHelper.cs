using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Helpers
{
    public interface IMisaApiTrackerHelper
    {
        void SaveLog(Database.MisaApiTracker model);
    }
}
