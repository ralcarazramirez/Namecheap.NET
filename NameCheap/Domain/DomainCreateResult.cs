﻿using System.Xml.Serialization;

namespace NameCheap.Domain;

[XmlRoot("DomainCreateResult")]
public class DomainCreateResult
{
    [XmlAttribute]
    public string Domain { get; set; }
    [XmlAttribute]
    public bool Registered { get; set; }
    [XmlAttribute]
    public double ChargedAmount { get; set; }
    [XmlAttribute]
    public int DomainID { get; set; }
    [XmlAttribute]
    public int OrderID { get; set; }
    [XmlAttribute]
    public int TransactionID { get; set; }
    [XmlAttribute]
    public bool WhoisguardEnable { get; set; }
    [XmlAttribute]
    public bool NonRealTimeDomain { get; set; }
}