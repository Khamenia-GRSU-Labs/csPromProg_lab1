using System.IO;
using System.Text;

namespace lab1;

class Program
{
    struct Command
        {
            public enum Operation { Search, Diff, Mode };
            public Operation operation;
            public string[] parametrs;
        };

        struct Gene_data
        {
            public string protein;
            public string organism;
            public string amino;
        };

        static string RLDecode(string sequance)
        {
            StringBuilder decoded = new();
            for (int i = 0; i < sequance.Length; i++)
            {
                int count = 1;
                if (sequance[i] >= '3' && sequance[i] <= '9')
                { count = sequance[i++] - '0'; }
                decoded.Append(sequance[i], count);
            }
            return decoded.ToString();
        }

        static Command[] ReadCommands(string path)
        {
            return File.ReadLines(path)
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .Select(line =>
                        {
                            string[] tokens = line.Split('\t');
                            return new Command
                            {
                                operation = tokens[0] switch
                                {
                                    "search" => Command.Operation.Search,
                                    "diff" => Command.Operation.Diff,
                                    "mode" => Command.Operation.Mode,
                                    _ => throw new ArgumentException($"Unknown operation: {tokens[0]}")
                                },
                                parametrs = tokens.Length > 1 ? tokens[1..] : Array.Empty<string>()
                            };
                        })
                        .ToArray();
        }

        static Gene_data[] ReadSequances(string path)
        {
            return File.ReadLines(path)
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .Select(line =>
                        {
                            string[] tokens = line.Split('\t');
                            return new Gene_data
                            {
                                protein = tokens[0],
                                organism = tokens.Length > 1 ? tokens[1] : string.Empty,
                                amino = tokens.Length > 2 ? RLDecode(tokens[2]) : string.Empty
                            };
                        })
                        .ToArray();
        }


    public static void Main()
    {


    }
}
