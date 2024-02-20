namespace Nop.Plugin.Widgets.PharmaBase.Models.Order
{
    public class MarketPlaceOrderDetail
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string MarketPlaceType { get; set; }
        public string CurrencyCode { get; set; }
        public int CargoTrackingNumber { get; set; }
        public int TotalPrice { get; set; }
        public string? Status { get; internal set; }
        public string? CargoProviderName { get; internal set; }
        public string? CargoTrackingLink { get; internal set; }
    }
}
