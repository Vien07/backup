using Steam.Core.Utilities.STeamHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Utilities.STeamHelper
{
    public interface IRandomString
    {
        public string GenerateRandomString(int length, int specialCharactersCount);
    }
}
