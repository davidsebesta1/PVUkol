using Newtonsoft.Json;
using System.Text;

namespace PVUkol.Handlers.Objects
{
    [Serializable]
    public class Stash : IEquatable<Stash?>
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public Dictionary<string, int> FindChancesByName { get; private set; }

        [JsonConstructor]
        public Stash(int id, string name, Dictionary<string, int> chances)
        {
            ID = id;
            Name = name;
            FindChancesByName = chances;
        }

        public Stash()
        {

        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Stash);
        }

        public bool Equals(Stash? other)
        {
            return other is not null && ID == other.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public static bool operator ==(Stash? left, Stash? right)
        {
            return EqualityComparer<Stash>.Default.Equals(left, right);
        }

        public static bool operator !=(Stash? left, Stash? right)
        {
            return !(left == right);
        }

        public override string? ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"ID: {ID} ");
            builder.AppendLine($"Name: {Name}");

            builder.Append("Chances: ");
            foreach (var kvp in FindChancesByName)
            {
                builder.Append(kvp.Key + ": " + kvp.Value + " ");
            }

            return builder.ToString();
        }
    }
}