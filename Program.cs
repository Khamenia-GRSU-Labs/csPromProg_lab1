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



    public static void Main()
    {


    }
}
