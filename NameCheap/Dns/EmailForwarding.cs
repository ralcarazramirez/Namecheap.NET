using System.Xml.Serialization;

namespace NameCheap.Dns;

public class EmailForwarding
{
    [XmlAttribute("mailbox")]
    public string? MailBox { get; set; }
    [XmlText]
    public string? ForwardTo { get; set; }
}