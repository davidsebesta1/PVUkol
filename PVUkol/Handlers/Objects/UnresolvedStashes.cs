using Newtonsoft.Json;

namespace PVUkol.Handlers.Objects
{
    public class UnresolvedStashes
    {
        public List<string> Friends { get; private set; }
        public List<Stash> Stashes { get; private set; }

        [JsonConstructor]
        public UnresolvedStashes(List<string> friends, List<Stash> stashes)
        {
            Friends = friends;
            Stashes = stashes;
        }

        public UnresolvedStashes()
        {

        }
    }
}