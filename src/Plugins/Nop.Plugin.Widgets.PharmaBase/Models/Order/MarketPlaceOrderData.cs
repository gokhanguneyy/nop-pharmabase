using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PharmaBase.Models.Order
{
    public class MarketPlaceOrderData
    {
        public IList<MarketPlaceOrderDetail> Data { get; set; }
        public int totalRecords { get; set; }
        public MarketPlaceOrderData()
        {
            Data = new List<MarketPlaceOrderDetail>();
        }
    }
}
