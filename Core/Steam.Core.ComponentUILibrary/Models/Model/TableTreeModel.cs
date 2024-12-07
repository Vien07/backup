using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.WebSockets;
using System.Reflection;

namespace ComponentUILibrary.Models
{
    public class TableTreeModel: MasterModel
    {

        public string ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string ActionMove { get; set; }
        public string ActionUpdateParent { get; set; }
        public string ActionEdit { get; set; }
        public string ActionDelete { get; set; }
        public string ActionEnable { get; set; }
        public List<TableTreeData> Data { get; set; } = new List<TableTreeData>();
        public string TableTreeDataJson()
        {
            var jsonString = JsonSerializer.Serialize(this.Data);

            return jsonString;
        }
        public void MapToObj(dynamic obj, string n_id, string n_title, string n_parentid, string n_order_num,string n_checked)
        {
            List<TableTreeData> rs = new List<TableTreeData>();
            foreach (var item in obj)
            {
                try
                {
                    TableTreeData temp = new TableTreeData();

                    PropertyInfo pi_n_id = item.GetType().GetProperty(n_id);
                    PropertyInfo pi_n_title = item.GetType().GetProperty(n_title);
                    PropertyInfo pi_n_parentid = item.GetType().GetProperty(n_parentid);
                    PropertyInfo pi_n_order_num = item.GetType().GetProperty(n_order_num);
                    PropertyInfo pi_n_checked = item.GetType().GetProperty(n_checked);

                    // Check if the property exists
                    if (pi_n_id != null)
                    {
                        // Get the property value
                        temp.n_id = Convert.ToInt64(pi_n_id.GetValue(item).ToString());
                        temp.n_parentid = Convert.ToInt64(pi_n_parentid.GetValue(item).ToString());
                        temp.n_order_num = Convert.ToDouble(pi_n_order_num.GetValue(item).ToString());
                        temp.n_title = pi_n_title.GetValue(item).ToString();
                        temp.n_checkStatus = pi_n_checked.GetValue(item);
                    }
                    rs.Add(temp);

                }
                catch (Exception ex)
                {

                }

            }
            this.Data = rs;
        }
    }
    public class TableTreeData
    {

        public long n_id { get; set; }
        public string n_title { get; set; }
        public long n_parentid { get; set; }
        public double n_order_num { get; set; }
        public bool n_checkStatus { get; set; }


    }
}