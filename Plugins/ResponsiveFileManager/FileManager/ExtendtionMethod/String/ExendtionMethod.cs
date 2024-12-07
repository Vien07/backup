
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

using System.Text.RegularExpressions;

namespace Steam.Core.FileManager.ExendtionMethod
{
    public static partial class ExendtionMethod
    {
        public static string ToJson(this object obj)
        {
            if (obj != null)
            {
                string result = JsonSerializer.Serialize(obj);

                return result;
            }
            else
            {
                return "Null";
            }

        }
        public static string GetFileExtension(this System.Drawing.Image image)
        {
            ImageFormat imageFormat = image.RawFormat;

            if (imageFormat.Equals(ImageFormat.Jpeg))
            {
                return ".jpg";
            }
            else if (imageFormat.Equals(ImageFormat.Png))
            {
                return ".png";
            }
            else if (imageFormat.Equals(ImageFormat.Gif))
            {
                return ".gif";
            }
            else if (imageFormat.Equals(ImageFormat.Bmp))
            {
                return ".bmp";
            }
            else if (imageFormat.Equals(ImageFormat.Tiff))
            {
                return ".tiff";
            }
            else if (imageFormat.Equals(ImageFormat.Icon))
            {
                return ".ico";
            }    
            else if (imageFormat.Equals(ImageFormat.Webp))
            {
                return ".webp";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string ToReplicationString(this object obj, int loop)
        {
            string result = obj.ToString();
            for (int i = 0; i < loop; i++)
            {
                result += obj;
            }
            if (loop == 0)
            {
                return "";
            }
            else
            {
                return result;

            }

        }
        public static string ToRemoveBreakSympol(this object obj)
        {
            string result = obj.ToString();
            try
            {
                result = result.Replace("\\n", "").Replace("\\t", "").Replace("\\\"", "\"");

            }
            catch (Exception ex)
            {
                result = obj.ToString();
            }
            return result;

        }
        public static string RemoveATags(this string input)
        {
            try
            {
                string pattern = @"<a>\s*</a>";
                return Regex.Replace(input, pattern, string.Empty);
            }
            catch (Exception)
            {

                return input;
            }

        }
        public static string FormatToMoney(this decimal money)
        {
            try
            {
                return money.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));

            }
            catch (Exception ex)
            {

                return money.ToString();

            }

        }
        public static string ToSlug(this string s)
        {
            try
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = s.Normalize(NormalizationForm.FormD);
                string tempString = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
                string rs = tempString.ToLower().Trim().Replace(" ", "-");
                char[] chars = @"$%#@ø–!*?;:~`+=()[]{}|\'<>,“”/^&"".&‘’‚“”„†‡‰‹›♠♣♥♦←↑→↓™!#$%'()*+,-./:;<=>?@[\]^_`{|}~-–—-¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿þÿΑαΒβΓγΔδΕεΖζΗηΘθΙιΚκΛλΜμΝνΞξΟοΠπΡρΣσΤτΥυΦφΧχΨψΩω●•".ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    string strChar = chars.GetValue(i).ToString();
                    if (rs.Contains(strChar))
                    {
                        rs = rs.Replace(strChar, "-");
                    }
                }
                rs = rs.Replace(" ", "-");
                var arr = rs.ToCharArray();
                int kt = 0;
                int tempPos = 2;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (kt > 1)
                    {
                        rs = rs.Remove(i - tempPos, 1);
                        tempPos++;
                    }

                    if (arr[i].ToString() == "-")
                    {
                        kt++;
                    }
                    else
                    {
                        kt = 0;
                    }
                }
                while (rs.ToCharArray()[rs.ToCharArray().Length - 1].ToString() == "-")
                {
                    var rsArr = rs.ToCharArray();
                    if (rsArr[rsArr.Length - 1].ToString() == "-")
                    {
                        rs = rs.Remove(rsArr.Length - 1, 1);
                    }
                    if (rsArr[0].ToString() == "-")
                    {
                        rs = rs.Remove(0, 1);
                    }
                }
                return rs;
            }
            catch (Exception)
            {

                return "";
            }

        }
        public static string GetExtension(this string s)
        {
            try
            {
                var lastDotIndex = s.LastIndexOf('.');
                if (lastDotIndex >= 0)
                {
                    var fileExtension = s.Substring(lastDotIndex + 1);
                    return fileExtension;
                }
                return "";

            }
            catch (Exception)
            {

                return "";
            }

        }
        public static string CheckExistsImage(this string imagePath)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + imagePath);
                if (File.Exists(filePath))
                {
                    return imagePath;
                }
                else
                {
                    return "";
                }


            }
            catch (Exception)
            {

                return "";
            }

        }
        public static bool isExistsImage(this string imagePath)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + imagePath);
                if (File.Exists(filePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {

                return false;
            }

        }
        public static string RemoveSign4VietnameseString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] VietnameseSigns = new string[] {

                    "aAeEoOuUiIdDyY",

                    "áàạảãâấầậẩẫăắằặẳẵ",

                    "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

                    "éèẹẻẽêếềệểễ",

                    "ÉÈẸẺẼÊẾỀỆỂỄ",

                    "óòọỏõôốồộổỗơớờợởỡ",

                    "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

                    "úùụủũưứừựửữ",

                    "ÚÙỤỦŨƯỨỪỰỬỮ",

                    "íìịỉĩ",

                    "ÍÌỊỈĨ",

                    "đ",

                    "Đ",

                    "ýỳỵỷỹ",

                    "ÝỲỴỶỸ"
                };

                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
                return str;
            }
            return "";
        }
        public static string ToUniqeID(this object st)
        {
            return st.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                       DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                       DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }
        public static string AddInfoFromKey(this string content, dynamic listKey)
        {
            try
            {
                foreach (var item in listKey)
                {
                    content = content.Replace(item.Key, item.Value);
                }
                return content;
            }
            catch (Exception ex)
            {

                return content;
            }

        }
    }
}
