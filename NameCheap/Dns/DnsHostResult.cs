﻿using System.Xml.Serialization;

namespace NameCheap.Dns;

[XmlRoot("DomainDNSGetHostsResult")]
public class DnsHostResult
{
    [XmlAttribute("IsUsingOurDNS")]
    public bool IsUsingOurDns { get; set; }

    [XmlElement("host")]
    public HostEntry[] HostEntries { get; set; }
}