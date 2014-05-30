/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

using DcrDataAccess.Models.OrderEntry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DcrDataAccess
{
    public class OrderEntryService : DataService, IDisposable
    {
        public OrderEntryService()
            : base()
        { }

        public OrderEntryService(string server, string db)
            : base(server, db)
        { }

        public OrderEntryService(string server, string db, string user, string pass)
            : base(server, db, user, pass)
        { }

        public List<OeLineItem> GetOeLineItems(string order_no)
        {
            List<OeLineItem> results = new List<OeLineItem>();

            string sql = @"
select		oe_line_uid,
			inv_mast_uid,
			unit_quantity,
			isnull(unit_price, 0) as unit_price,
			isnull(disposition, '') as disposition
from		p21_view_oe_line
where		unit_quantity > 0
			and parent_oe_line_uid = 0
			and isnull(disposition, '') <> 'C'
			and delete_flag = 'N'
			and isnull(cancel_flag, 'N') = 'N'
			and order_no = @order_no
order by	line_no
";

            using (var cmd = new SqlCommand(sql, SqlConn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@order_no", order_no);

                try
                {
                    SqlConn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OeLineItem item = new OeLineItem();
                            item.oe_line_uid = reader.GetInt32(0);
                            item.inv_mast_uid = reader.GetInt32(1);
                            item.unit_quantity = reader.GetDecimal(2);
                            item.unit_price = reader.GetDecimal(3);
                            item.disposition = reader.GetString(4);

                            results.Add(item);
                        }
                    }
                }
                catch { throw; }
                finally
                {
                    SqlConn.Close();
                }
            }

            return results;
        }

        public void SaveLinesFile(string order_no, List<OeLineItem> items)
        {
            try
            {
                string path = Path.GetTempPath();
                string filename = Path.Combine(path, order_no + ".json");

                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(filename, JsonConvert.SerializeObject(items));
            }
            catch { throw; }
        }

        public void DeleteFile(string order_no)
        {
            try
            {
                string path = Path.GetTempPath();
                string filename = Path.Combine(path, order_no + ".json");

                if (File.Exists(filename))
                    File.Delete(filename);                
            }
            catch { throw; }
        }

        public List<OeLineItem> LoadLinesFile(string order_no)
        {
            try
            {
                string path = Path.GetTempPath();
                string filename = Path.Combine(path, order_no + ".json");

                if (File.Exists(filename))
                    return JsonConvert.DeserializeObject<List<OeLineItem>>(File.ReadAllText(filename));
            }
            catch { throw; }
            return new List<OeLineItem>();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
