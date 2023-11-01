using System.Xml.Serialization;

namespace NameCheap.Domain;

[XmlRoot("Tlds")]
public class TldListResult
{
    [XmlElement("Tld")]
    public Tld[] Tlds { get; set; }
}