using WebSiteJeBouniqueV_2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSiteJeBouniqueV_2.Models.Home
{
    public class Item
    {
        public Tbl_Product Product { get; set; }
        public int Quantity { get; set; }
    }
}