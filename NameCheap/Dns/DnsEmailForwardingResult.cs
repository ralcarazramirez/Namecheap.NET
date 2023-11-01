using System.Xml.Serialization;

namespace NameCheap.Dns;

[XmlRoot("DomainDNSGetEmailForwardingResult")]
public class DnsEmailForwardingResult
{
    [XmlElement("Forward")]
    public List<EmailForwarding> Emails { get; set; }
}