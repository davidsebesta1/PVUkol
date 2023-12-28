using PVUkol.Extensions;
using PVUkol.Handlers.Objects;
using System.Diagnostics;

namespace PVUkol.Handlers
{
    public class GiftHideHandler
    {
        private readonly UnresolvedStashes _unresolvedStashes;
        private readonly Dictionary<string, List<Stash>> _rows;
        private readonly Dictionary<string, Stash> _choosenStashes;
        private readonly HashSet<Stash> _takenStashes;

        private int _curProcessingIndex = 0;

        public readonly bool Debug;

        public GiftHideHandler(UnresolvedStashes unresolvedStashes, bool debug = false)
        {
            Debug = debug;
            _unresolvedStashes = unresolvedStashes;
            _rows = new Dictionary<string, List<Stash>>(_unresolvedStashes.Friends.Count);
            _choosenStashes = new Dictionary<string, Stash?>(_unresolvedStashes.Friends.Count);
            _takenStashes = new HashSet<Stash>(_unresolvedStashes.Stashes.Count);

            foreach (string friend in _unresolvedStashes.Friends)
            {
                List<Stash> stashes = [.. _unresolvedStashes.Stashes.OrderBy(n => n.FindChancesByName[friend])];
                _rows.Add(friend, stashes);

                _choosenStashes.Add(friend, null);
            }

        }

        public Dictionary<string, Stash> Solve(out TimeSpan elapsed)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (_choosenStashes.Values.Any(n => n == null))
            {
                TryGetUnsolvedColumnAtIndex(_curProcessingIndex, out Dictionary<string, Stash> column);

                SolveForColumn(column);
                _curProcessingIndex++;

                if (Debug)
                {
                    foreach (var kvp in _choosenStashes)
                    {
                        Console.WriteLine($"{kvp.Key}: {(kvp.Value == null ? "null" : kvp.Value.Name)}");
                    }
                }
            }

            stopwatch.Stop();
            elapsed = stopwatch.Elapsed;
            return _choosenStashes;
        }

        private bool TryGetUnsolvedColumnAtIndex(int index, out Dictionary<string, Stash> column)
        {
            if (index >= 0 && index < _rows.Count)
            {
                column = _rows.Where(n => _choosenStashes[n.Key] == null).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ElementAtOrDefault(index));
                return true;
            }

            column = null;
            return false;
        }

        private void SolveNonDuplicite(Dictionary<string, Stash> nonDuplicites)
        {
            if (Debug)
            {
                ConsoleExtension.WriteLine("Not duplicite values:", ConsoleColor.Green);
                foreach (var t in nonDuplicites)
                {
                    Console.Write(t.Value);
                }
            }
            foreach (var kvp in nonDuplicites)
            {
                _choosenStashes[kvp.Key] = kvp.Value;
                _takenStashes.Add(kvp.Value);
            }
        }

        private void SolveDuplicite(Dictionary<Stash, List<string>> duplicites)
        {

            if (Debug)
            {
                foreach (var t in duplicites)
                {
                    Console.WriteLine($"{t.Key}\nNames: ");
                    foreach (var name in t.Value)
                    {
                        Console.WriteLine(name);
                    }
                }
            }

            foreach (var duplicite in duplicites)
            {
                string? min = null;
                if (TryGetUnsolvedColumnAtIndex(_curProcessingIndex + 1, out Dictionary<string, Stash> nextColumn))
                {
                    min = duplicite.Value.MaxBy(n => duplicite.Key.FindChancesByName[n] + nextColumn[n].FindChancesByName[n]);
                }
                else
                {
                    min = duplicite.Value.MaxBy(n => duplicite.Key.FindChancesByName[n]);
                }

                if (min == null)
                {
                    throw new InvalidOperationException("Minimum value is null");
                }
                if (Debug) ConsoleExtension.WriteLine($"Name with minimum chance for {duplicite.Key.ID} ({duplicite.Key.Name}): {min}", ConsoleColor.Green);

                _choosenStashes[min] = duplicite.Key;
                _takenStashes.Add(duplicite.Key);
            }
        }

        private void SolveForColumn(Dictionary<string, Stash> column)
        {
            if (Debug) ConsoleExtension.WriteLine($"Solving for column at index {_curProcessingIndex}", ConsoleColor.Magenta);

            if (Debug) ConsoleExtension.WriteLine("Non duplicites:", ConsoleColor.Green);
            Dictionary<string, Stash> nonDuplicite = column.Where(n => !_takenStashes.Contains(n.Value))
                .OrderBy(name => _rows[name.Key].Average(stash => stash.FindChancesByName[name.Key]))
                .GroupBy(n => n.Value).Where(pair => pair.Count() == 1)
                .SelectMany(pair => pair)
                .ToDictionary(name => name.Key, stash => stash.Value);

            SolveNonDuplicite(nonDuplicite);

            if (Debug) ConsoleExtension.WriteLine("Duplicites:", ConsoleColor.Green);
            var alldups = column.Where(n => !nonDuplicite.ContainsValue(n.Value));
            Dictionary<Stash, List<string>> dupliciteStashes = alldups.Where(n => !_takenStashes.Contains(n.Value))
                .GroupBy(pair => pair.Value)
                .ToDictionary(group => group.Key, groupItems => groupItems.Select(groupItem => groupItem.Key).ToList());

            SolveDuplicite(dupliciteStashes);
        }
    }
}
