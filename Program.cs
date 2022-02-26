using System;

namespace Memory
{
    class Matrix
    {
        public Matrix(int level, string[] words)
        {
            lenght = theLongestWord(words);
            
            switch (level)
            {
                case 1:
                    height = 2;
                    width = 4;
                    break;
                case 2:
                    height = 4;
                    width = 4;
                    break;
            }
            Print();
        }

        private void Print()
        {
            int number = 0;
            string str;
            Console.Clear();

            for (int k = 0; k < height; k++)
            {
                PrintLineHorizontal();
                PrintLineVertical();

                for (int j = 0; j < width; j++)
                {
                    Console.Write("|");
                    for (int i = 0; i < lenght / 2; i++)
                        Console.Write(" ");
                    str = number.ToString();
                    Console.Write(str);
                    for (int i = 0; i < (lenght / 2) - str.Length + 1; i++)
                        Console.Write(" ");
                    number++;
                }
                Console.Write("|\n");
                PrintLineVertical();
            }

            PrintLineHorizontal();
        }

        private void PrintLineHorizontal()
        {
            Console.Write(" ");
            for (int i = 0; i < lenght * width + width - 1; i++)
                Console.Write("-");
            Console.Write("\n");
        }

        private void PrintLineVertical()
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write("|");
                for (int i = 0; i < lenght; i++)
                    Console.Write(" ");
            }
            Console.Write("|\n");
        }

        private int height, width, lenght;

        static int theLongestWord(string[] lines)
        {
            int max = 0;
            foreach (string line in lines)
            {
                if (line.Length > max)
                    max = line.Length;
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
            
            Matrix Memory = new Matrix(difficulty, lines);
            
        }
    }
}
