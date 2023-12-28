using PVUkol.Extensions;
using PVUkol.FileHandlers;
using PVUkol.Handlers;
using PVUkol.Handlers.Objects;

namespace PVUkol
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                try
                {
                    UnresolvedStashes stashes = DeserializeStashFromFile(args[0]);
                    ResolvedStashes solution = Solve(stashes);
                    SerializeProcess(solution);

                    Console.WriteLine("Done! Press any key to exit");
                    Console.ReadLine();
                }
                catch (FileLoadException)
                {
                    ConsoleExtension.WriteLine("Specified file has unrecongized extension", ConsoleColor.Red);
                }
                catch (Exception ex)
                {
                    ConsoleExtension.WriteLine($"Error has occured: {ex}", ConsoleColor.Red);
                }

            }
            else if (args.Length == 2)
            {
                try
                {
                    UnresolvedStashes stashes = DeserializeStashFromFile(args[0]);

                    if (args[1] == "debug")
                    {
                        ResolvedStashes solution = Solve(stashes, true);
                        SerializeProcess(solution);

                        Console.WriteLine("Done! Press any key to exit");
                        Console.ReadLine();
                    }

                }
                catch (FileLoadException)
                {
                    ConsoleExtension.WriteLine("Specified file has unrecongized extension", ConsoleColor.Red);
                }
                catch (Exception ex)
                {
                    ConsoleExtension.WriteLine($"Error has occured: {ex}", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtension.WriteLine("No file provided... press any key to continue", ConsoleColor.Red);
                Console.ReadLine();
            }
        }

        private static ResolvedStashes Solve(UnresolvedStashes stashes, bool debug = false)
        {
            Console.WriteLine("File deserialized... running solving algorithm");
            GiftHideHandler handler = new GiftHideHandler(stashes, debug);
            Dictionary<string, Stash> solution = handler.Solve(out TimeSpan elapsed);

            ConsoleExtension.WriteLine($"Found solution for your stashes in {EvualuateElapsed(elapsed)}: ", ConsoleColor.Green);
            foreach (var pair in solution)
            {
                Console.Write(pair.Key + " - ");
                ConsoleExtension.Write($"{pair.Value.Name} ", ConsoleColor.Green);
                ConsoleExtension.WriteLine($"({pair.Value.FindChancesByName[pair.Key]}%)", ConsoleColor.Yellow);
            }

            return new ResolvedStashes(solution.ToDictionary(n => n.Key, m => m.Value.Name));
        }

        private static string EvualuateElapsed(TimeSpan elapsed)
        {
            if (elapsed.TotalSeconds >= 1d)
            {
                return Math.Round(elapsed.TotalSeconds, 2).ToString() + "s";
            }

            return Math.Round(elapsed.TotalMilliseconds, 2).ToString() + "ms";
        }

        private static void SerializeProcess(ResolvedStashes solution)
        {
            ConsoleExtension.WriteLine($"In which format you would like to save your solution?\n1. JSON\n2. XML\n3. YAML\n4. None\nPlease type a number if your selection: ", ConsoleColor.Yellow);

            string input;
            while ((input = Console.ReadLine()) != null)
            {
                while (int.TryParse(input, out int result) && (result > 0 && result <= 4))
                {
                    switch (result)
                    {
                        case 1:
                            Console.WriteLine("Serializing in JSON...");
                            JsonFileHandler.Serialize("Result.json", solution);
                            return;
                        case 2:
                            Console.WriteLine("Serializing in XML...");
                            XmlFileHandler.Serialize<ResolvedStashes>("Result.xml", solution);
                            return;
                        case 3:
                            Console.WriteLine("Serializing in YAML...");
                            YamlFileHandler.Serialize("Result.yaml", solution);
                            return;
                        case 4:
                            Console.WriteLine("No saving selected");
                            return;
                    }
                    ConsoleExtension.Write("Invalid input", ConsoleColor.Red);
                }
            }
        }

        private static UnresolvedStashes? DeserializeStashFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Specified file not found ({path})");
            }

            string extension = Path.GetExtension(path);
            return extension switch
            {
                ".yaml" => YamlFileHandler.Deserialize<UnresolvedStashes>(path),
                ".json" => JsonFileHandler.Deserialize<UnresolvedStashes>(path),
                ".xml" => XmlFileHandler.Deserialize<UnresolvedStashes>(path),
                _ => throw new FileLoadException("Specified file has unrecognized file extension"),
            };
        }
    }
}