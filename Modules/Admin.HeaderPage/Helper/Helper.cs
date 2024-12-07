using Admin.HeaderPage.Database;
using Microsoft.AspNetCore.Http;
using Steam.Core.Common.Models;
using Steam.Core.Common.SteamModels;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using System.Drawing;

namespace Admin.HeaderPage
{
    public class HeaderPageHelper
    {
        public HeaderPageHelper()
        {
        }
        public string GenerateHeader(List<Menu> listMenu, List<MenuItemStyle> listMenuItemStyle, List<MenuStyle> listMenuStyle, Database.HeaderPage headerPage)
        {
            try
            {
                string header = "";
                header = headerPage.HeaderBlock;

                string menu = "";
                var menuItemBlock = "";

                foreach (var item in listMenu.Where(p => p.ParentID == 0))
                {
                    var menuStyle = listMenuStyle.Where(p => p.Pid == item.MenuStylePid).FirstOrDefault();
                    menu = menuStyle.MenuBlock;

                    var listChild = listMenu.Where(p => p.ParentID == item.Pid).ToList();
                    var tempMenuItemBlock = listMenuItemStyle.Where(p => p.Level == item.ShowLevel).FirstOrDefault().ItemBlock;
                    tempMenuItemBlock = tempMenuItemBlock.Replace("{{MENU_NAME}}", item.Title).Replace("{{MENU_URL}}", item.URL);
                    string menuItem = "";
                    if (listChild.Count > 0)
                    {
                        menuItem += GenerateMenu(listChild, listMenuItemStyle, listMenu);
                    }
                    menuItemBlock += tempMenuItemBlock.Replace("{{MENU_ITEM}}", menuItem);

                }
                menu = menu.Replace("{{MENU_ITEM}}", menuItemBlock);
                header = header.Replace("{{MENU}}", menu);
                //SaveFileHeader(header);
                return header;

            }
            catch (Exception ex)
            {
                return "----Có lỗi trong quá trình tạo header----";
            }
        }

        string GenerateMenu(List<Menu> listMenu, List<MenuItemStyle> listMenuItemStyle, List<Menu> listMenuMaster)
        {
            var menuItemBlock = "";

            try
            {
                foreach (var item in listMenu)
                {
                    var listChild = listMenuMaster.Where(p => p.ParentID == item.Pid).ToList();
                    var tempMenuItemBlock = listMenuItemStyle.Where(p => p.Level == item.ShowLevel).FirstOrDefault().ItemBlock;
                    tempMenuItemBlock = tempMenuItemBlock.Replace("{{MENU_NAME}}", item.Title).Replace("{{MENU_URL}}", item.URL);
                    string menuItem = "";

                    if (listChild.Count > 0)
                    {
                        menuItem += GenerateMenu(listChild, listMenuItemStyle, listMenu);

                    }

                    menuItemBlock += tempMenuItemBlock.Replace("{{MENU_ITEM}}", menuItem);

                }

            }
            catch (Exception ex)
            {
                return "";
            }
            menuItemBlock = menuItemBlock.Replace("{{MENU_ITEM}}", "");

            return menuItemBlock;

        }
        void SaveFileHeader(string content)
        {
            string fileName = "header.html"; // Replace with the name of the file you want to update

            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\layout\\" + fileName);

            if (File.Exists(filePath))
            {

                File.WriteAllText(filePath, content);

            }

        }

    }
}
