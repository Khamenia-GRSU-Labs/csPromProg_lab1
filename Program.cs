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


    public static void Main()
    {


    }
}
