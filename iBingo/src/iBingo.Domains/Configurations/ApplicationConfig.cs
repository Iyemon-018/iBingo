namespace iBingo.Domains.Configurations
{
    using System.Xml.Serialization;

    public class ApplicationConfig
    {
        [XmlElement(nameof(Shuffle))]
        public ShuffleConfig Shuffle { get; set; }
    }
}