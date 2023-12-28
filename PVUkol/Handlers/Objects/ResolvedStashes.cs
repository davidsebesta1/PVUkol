
using System.Xml.Serialization;

namespace PVUkol.Handlers.Objects
{
    /// <summary>
    /// A class containing information for solved problem
    /// </summary>
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

    /// <summary>
    /// Class made only for XML serialization since XML cannot serialize dictionary by default
    /// </summary>
    [Serializable]
    public class StashItem
    {
        [XmlAttribute]
        public string PersonName { get; set; }

        [XmlText]
        public string StashName { get; set; }
    }
}