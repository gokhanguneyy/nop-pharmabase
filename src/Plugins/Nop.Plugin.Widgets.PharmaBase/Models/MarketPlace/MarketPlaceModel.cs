using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace
{
    public class MarketPlaceModel
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string MarketPlaceSupplierId { get; set; }
        public bool IsActive { get; set; }

        public int MarketPlaceId { get; set; }  
    }
}
