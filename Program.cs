using System;

namespace Memory
{
    class Matrix
    {
        public Matrix(int level, string[] words)
        {
            Console.WriteLine(theLongestWord(words));
            //int height; int width;
        }

        static int theLongestWord(string[] lines)
        {
            int max = 0;
            foreach (string line in lines)
            {
                if (line.Length > max)
                {
                    max = line.Length;
                }

                Console.WriteLine(line); //just to be sure
                Console.WriteLine("\t" + line.Length);
            }
            return max;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string file = "Words.txt";
            string[] lines = System.IO.File.ReadAllLines(file);
            int difficulty = 0;

            Console.WriteLine("Welcome to memory game!\n");
            Console.WriteLine("Select difficulty level:\n1 - Easy\t\t2 - Hard");

            do
            {
                var input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        difficulty = 1;
                        break;
                    case ConsoleKey.D2:
                        difficulty = 2;
                        break;
                    default:
                        Console.WriteLine("\nChoose one of the available options");
                        break;
                }
            } while (difficulty == 0);
            Console.WriteLine(difficulty);
            //Matrix Memory = new Matrix(dificulty, lines);
            
        }
    }
}
