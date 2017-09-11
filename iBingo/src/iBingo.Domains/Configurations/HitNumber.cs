namespace iBingo.Domains.Configurations
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class HitNumber
    {
        [XmlText]
        public int Number { get; set; }
    }
}