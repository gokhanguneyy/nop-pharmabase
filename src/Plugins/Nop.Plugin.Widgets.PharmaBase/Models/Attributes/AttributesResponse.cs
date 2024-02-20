namespace Nop.Plugin.Widgets.PharmaBase.Models.Attributes
{
    public class Attributes
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool Mandatory { get; set; }
        public string Type { get; set; }
        public bool MultiValue { get; set; }
    }

    public class BaseAttribute
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool Mandatory { get; set; }
        public string Type { get; set; }
        public bool MultiValue { get; set; }
    }

    public class AttributesData
    {
        public AttributesData()
        {
            BaseAttributes = new List<BaseAttribute>();
            Attributes = new List<Attributes>();
            VariantAttributes = new List<VariantAttribute>();
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<TCategoryAttribute> CategoryAttributes { get; set; }
        public List<BaseAttribute> BaseAttributes { get; set; }
        public List<Attributes> Attributes { get; set; }
        public List<VariantAttribute> VariantAttributes { get; set; }
    }

    public class AttributesResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public int Version { get; set; }
        public string Message { get; set; }
        public List<AttributesData> Data { get; set; }

    }

    public class VariantAttribute
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool Mandatory { get; set; }
        public string Type { get; set; }
        public bool MultiValue { get; set; }
    }

    public class TCategoryAttribute
    {
        public int CategoryId { get; set; }
        public bool AllowCustom { get; set; }
        public bool Required { get; set; }
        public bool Slicer { get; set; }
        public bool Varianter { get; set; }
        public TAttribute Attribute { get; set; }
        public List<TAttributeValue> AttributeValues { get; set; }
    }

    public class TAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TAttributeValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
