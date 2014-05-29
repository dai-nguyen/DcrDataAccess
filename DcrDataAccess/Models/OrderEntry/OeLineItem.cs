/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

namespace DcrDataAccess.Models.OrderEntry
{
    public class OeLineItem
    {
        public int oe_line_uid { get; set; }
        public int inv_mast_uid { get; set; }
        public decimal unit_quantity { get; set; }
        public decimal unit_price { get; set; }
        public string disposition { get; set; }

        public OeLineItem()
        {
            oe_line_uid = -1;
            inv_mast_uid = -1;
            unit_quantity = -1;
            unit_price = -1;
            disposition = "";
        }

        public OeLineItem(string oe_line_uid,
            string inv_mast_uid,
            string unit_quantity,
            string unit_price,
            string disposition)
        {
            int ti = 0;
            decimal td = 0;

            this.oe_line_uid = int.TryParse(oe_line_uid, out ti) ? ti : -1;
            this.inv_mast_uid = int.TryParse(inv_mast_uid, out ti) ? ti : -1;
            this.unit_quantity = decimal.TryParse(unit_quantity, out td) ? td : -1;
            this.unit_price = decimal.TryParse(unit_price, out td) ? td : -1;
            this.disposition = disposition;
        }

        public void Copy(OeLineItem copy)
        {
            this.oe_line_uid = copy.oe_line_uid;
            this.inv_mast_uid = copy.inv_mast_uid;
            this.unit_quantity = copy.unit_quantity;
            this.unit_price = copy.unit_price;
            this.disposition = copy.disposition;
        }
    }
}
