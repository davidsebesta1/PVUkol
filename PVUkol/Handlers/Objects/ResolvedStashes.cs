
using System.Xml.Serialization;

namespace PVUkol.Handlers.Objects
{
    [Serializable]
    public class ResolvedStashes
    {
        [XmlIgnore]
        public Dictionary<string, string> Solution;

        [XmlElement("SolutionItem")]
        public List<StashItem> SolutionItems
        {
            get
            {
                return Solution?.Select(kv => new StashItem { PersonName = kv.Key, StashName = kv.Value }).ToList();
            }
            set
            {
                Solution = value?.ToDictionary(item => item.PersonName, item => item.StashName);
            }
        }

        public ResolvedStashes(Dictionary<string, string> solution)
        {
            Solution = solution;
        }

        public ResolvedStashes()
        {

        }
    }

    [Serializable]
    public class StashItem
    {
        [XmlAttribute]
        public string PersonName { get; set; }

        [XmlText]
        public string StashName { get; set; }
    }
}