using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ComponentUILibrary.Models
{
    public class TreeSelectModel: MasterModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<TreeSelectData> Data { get; set; } = new List<TreeSelectData>();
        public string Class { get; set; }
        public string SelectedValue { get; set; } = "";
        public int Searchable { get; set; } = 1;
        public string Placeholder { get; set; } = "Search...";
        public string EmptyFound { get; set; } = "No results found...";
        public int Disable { get; set; } = 0;
        public int OpenLevel { get; set; } = 0;
        public bool FirstLoadLib { get; set; } = false;
        public bool isIndependentNodes { get; set; } = true;
        public bool isSingleSelect { get; set; } = false;
        public bool isGrouped { get; set; } = true;

        public string TreeSelectDataJson()
        {
            try
            {
                if(this.Data != null)
                {
                    var jsonString = JsonSerializer.Serialize(this.Data);
                    return jsonString;

                }
                else
                {
                    return "[]";

                }

            }
            catch (Exception)
            {

                return "[]";
                
            }
  
        }
        public void MapToObj(dynamic obj, string n_id, string n_title, string n_parentid, string n_order_num, string n_checked, string? getAtParentId="")
        {
            List<TreeSelectData> rs = new List<TreeSelectData>();
            foreach (var item in obj)
            {
                try
                {
                    TreeSelectData temp = new TreeSelectData();

                    PropertyInfo pi_n_id = item.GetType().GetProperty(n_id);
                    PropertyInfo pi_n_title = item.GetType().GetProperty(n_title);
                    PropertyInfo pi_n_parentid = item.GetType().GetProperty(n_parentid);
                    PropertyInfo pi_n_order_num = item.GetType().GetProperty(n_order_num);
                    PropertyInfo pi_n_checked = item.GetType().GetProperty(n_checked);

                    // Check if the property exists
                    if (pi_n_id != null)
                    {
                        // Get the property value
                        //temp.value = Convert.ToInt64(pi_n_id.GetValue(item).ToString());
                        temp.value = pi_n_id.GetValue(item).ToString();
                        temp.name = pi_n_title.GetValue(item).ToString();
                        temp.parentId = pi_n_parentid.GetValue(item).ToString();
                    }
                    rs.Add(temp);

                }
                catch (Exception ex)
                {

                }

            }
            if(getAtParentId != null)
            {
                this.Data = MapDataToHierarchy(rs, getAtParentId);

            }
            else
            {
                this.Data = rs;
            }
        }

        private List<TreeSelectData> MapDataToHierarchy(List<TreeSelectData> data, string getAtParentId)
        {
            try
            {
                if (data == null || data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var dataMap = data.ToDictionary(item => item.value);

                    foreach (var item in data)
                    {
                        if (item.parentId != "0" && dataMap.ContainsKey(item.parentId))
                        {
                            var parent = dataMap[item.parentId];
                            parent.children.Add(item);
                        }
                    }
                    var rootNodes = data.Where(item => item.parentId == getAtParentId.ToString()).ToList();
                    return rootNodes;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
