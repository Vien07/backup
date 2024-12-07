using Steam.Core.Utilities.STeamHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Utilities.STeamHelper
{
    public class RamdomString : IRandomString
    {
        public string GenerateRandomString(int length, int specialCharactersCount)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string specialChars = "!@#$%^&*";

            StringBuilder result = new StringBuilder();
            int specialCharsAdded = 0;

            for (int i = 0; i < length; i++)
            {
                if (i < specialCharactersCount || specialCharsAdded < specialCharactersCount)
                {
                    // Chọn ngẫu nhiên ký tự đặc biệt
                    char randomChar = specialChars[random.Next(specialChars.Length)];
                    result.Append(randomChar);
                    specialCharsAdded++;
                }
                else
                {
                    // Chọn ngẫu nhiên ký tự bình thường
                    char randomChar = chars[random.Next(chars.Length)];
                    result.Append(randomChar);
                }
            }

            // Trộn chuỗi ngẫu nhiên để đảm bảo vị trí ký tự đặc biệt ngẫu nhiên
            string shuffledString = new string(result.ToString().OrderBy(x => random.Next()).ToArray());
            return shuffledString;
        }
    }
}
