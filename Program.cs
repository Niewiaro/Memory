using System;

namespace Memory
{
    class Program
    {
        static int theLongestWord(string[] lines)
        {
            int max = 0;
            foreach (string line in lines)
            {
                if (line.Length > max)
                {
                    max = line.Length;
                }

                Console.WriteLine(line);
                Console.WriteLine("\t" + line.Length);
            }
            return max;
        }


        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("Words.txt");

            Console.WriteLine(theLongestWord(lines));
            
        }
    }
}
