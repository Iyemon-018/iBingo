namespace iBingo.Domains.Configurations
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class HistoryConfig
    {
        [XmlElement(nameof(Create))]
        public string CreateString { get; set; }

        [XmlIgnore]
        public DateTime Create { get; set; }

        [XmlArray]
        public HitNumber[] HitNumbers { get; set; }

        public void BeginWrite() => CreateString = Create.ToString("o");

        public void AfterRead() => Create = DateTime.ParseExact(CreateString, "o", null);
    }
}