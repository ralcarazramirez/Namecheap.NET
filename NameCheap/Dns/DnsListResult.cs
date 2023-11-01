using System.Xml.Serialization;

namespace NameCheap.Dns;

[XmlRoot("DomainDNSGetListResult")]
public class DnsListResult
{
    [XmlAttribute("IsUsingOurDNS")]
    public bool IsUsingOurDns { get; set; }

    [XmlElement("Nameserver")]
    public List<string> NameServers { get; set; }
}