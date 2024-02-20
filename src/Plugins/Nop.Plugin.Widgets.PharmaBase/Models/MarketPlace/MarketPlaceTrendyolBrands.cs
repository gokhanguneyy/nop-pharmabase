using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandResponse
    {
        public BrandResponse()
        {
            Brands = new List<Brand>();
        }
        public List<Brand> Brands { get; set; }
    }
}
