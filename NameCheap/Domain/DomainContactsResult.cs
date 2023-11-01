﻿using System.Xml.Serialization;

namespace NameCheap.Domain;

[XmlRoot("DomainContactsResult")]
public class DomainContactsResult
{
    public ContactInformation Registrant { get; set; }
    public ContactInformation Tech { get; set; }
    public ContactInformation Admin { get; set; }
    public ContactInformation AuxBilling { get; set; }
}