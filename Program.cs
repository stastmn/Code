using System;

namespace refs
{
    internal static class Program
    {
        public static void Main(string[] gitDirectoryPath)
        {
            try
            {
                if (gitDirectoryPath.Length != 1) throw new Exception();
                RefsPrinter.PrintRefs(gitDirectoryPath[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}