using System.Xml.Serialization;

namespace NameCheap.Domain;

[XmlRoot("CommandResponse")]
public record DomainListResult
{
    [XmlArray("DomainGetListResult")]
    [XmlArrayItem("Domain", typeof(Domain))]
    public Domain[] Domains { get; set; }
}