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

        static string DoCommandOperatrion(Command command, Gene_data[] sequances)
        {
            StringBuilder result = new();

            switch (command.operation)
            {
                case Command.Operation.Search:
                    string amino_pattern = command.parametrs[0];
                    result.AppendLine($"search\t{amino_pattern}");
                    bool found = false;
                    foreach (var seq in sequances)
                    {
                        if (!seq.amino.Contains(amino_pattern)) continue;
                        found = true;
                        result.AppendLine($"{seq.organism}\t{seq.protein}");
                    }
                    if (!found) { result.AppendLine("NOT FOUND"); }
                    break;
                case Command.Operation.Diff:
                    string protein1 = command.parametrs[0], protein2 = command.parametrs[1];
                    result.AppendLine($"diff\t{protein1}\t{protein2} amino-acids differense:");
                    int p1 = Array.FindIndex(sequances, gd => gd.protein == protein1);
                    int p2 = Array.FindIndex(sequances, gd => gd.protein == protein2);
                    if (p1 == -1 || p2 == -1)
                    {
                        result.AppendLine("MISSING");
                        break;
                    }
                    string amino1 = sequances[p1].amino, amino2 = sequances[p2].amino;
                    int missmatch_count = 0;
                    for (int i = 0; i < amino1.Length && i < amino2.Length; i++)
                    {
                        if (amino1[i] != amino2[i]) missmatch_count++;
                    }
                    missmatch_count += Math.Abs(amino1.Length - amino2.Length);
                    result.AppendLine($"{missmatch_count}");
                    break;
                case Command.Operation.Mode:

                    result.AppendLine($"mode\t{command.parametrs[0]}");
                    result.AppendLine("Amino-acid occurs:");

                    int matched = Array.FindIndex(sequances, gd => gd.protein == command.parametrs[0]);
                    if (matched == -1)
                    { result.AppendLine("MISSING"); break; }

                    Dictionary<char, uint> amino_counts = new();
                    foreach (char amino_acid in sequances[matched].amino)
                    {
                        if (!amino_counts.ContainsKey(amino_acid))
                        {
                            amino_counts.Add(amino_acid, 0);
                        }
                        amino_counts[amino_acid]++;
                    }
                    KeyValuePair<char, uint> most_occured = amino_counts.First();
                    foreach (var (key, val) in amino_counts)
                    {
                        if (most_occured.Value < val) { most_occured = new(key, val); }
                    }
                    result.AppendLine($"{most_occured.Key}\t{most_occured.Value}");
                    break;
            }

            return result.ToString();
        }

    public static void Main()
    {
        Command[] commands = commands = ReadCommands(@"commands.txt");
        Gene_data[] sequances = sequances = ReadSequances(@"sequances.txt");

        StringBuilder genedata = new();

        for (uint i = 0; i < commands.Length; i++)
        {
            genedata.Append($"{i:000}\t");
            genedata.AppendLine(DoCommandOperatrion(commands[i], sequances));
        }
        File.WriteAllText("genedata.txt", genedata.ToString().AsSpan());
    }
}
