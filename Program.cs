using System;

namespace refs
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var path = "";
                if (args.Length == 1) path = args[0];
                RefsPrinter.PrintRefs(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}