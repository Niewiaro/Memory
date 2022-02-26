using System;
using System.Collections.Generic;
using System.Diagnostics;

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
                    chances = 10;
                    difficulty = "EASY";
                    break;
                case 2:
                    height = 4;
                    width = 4;
                    chances = 15;
                    difficulty = "HARD";
                    break;
            }
            trials = chances;

            int hw = height * width;
            int[] wordIndex = RandomNoDuplicate(hw / 2, words.Length);
            Array.Resize(ref positionIndex, hw);
            positionIndex = RandomNoDuplicate(hw, hw);
            for (int i = 0; i < positionIndex.Length; i++)
            {
                positionIndex[i]--;
            }
            Array.Resize(ref randomizedWords, hw);

            for (int i = 0, j = 0; i < hw; i++)
            {
                randomizedWords[positionIndex[i++]] = words[wordIndex[j]];
                randomizedWords[positionIndex[i]] = words[wordIndex[j++]];
            }

            Print();
        }

        public bool Play(int number)
        {
            sw.Start();
            if (number < 0 || number > width * height)
                return true;
            if (!list.Contains(number))
                list.Add(number);
            int listLenght = list.Count;
            int index = -1;

            Print(); //before deletion

            if (listLenght != 0 && listLenght % 2 == 0)
            {
                index = findIndex(list[listLenght - 1]);
                if (index % 2 == 0)
                {
                    if (findIndex(list[listLenght - 2]) != index + 1)
                    {
                        chances--;
                        Print();
                        list.RemoveAt(listLenght - 1);
                        list.RemoveAt(listLenght - 2);       
                    }
                }

                else
                {
                    if (findIndex(list[listLenght - 2]) != index - 1)
                    {
                        chances--;
                        Print();
                        list.RemoveAt(listLenght - 1);
                        list.RemoveAt(listLenght - 2);         
                    }
                }
            }
            if (list.Count == width * height)
            {
                sw.Stop();
                Console.WriteLine("\nYou won! :)");  
                Console.WriteLine("Your time was {0}", sw.Elapsed);
                Console.WriteLine("You solved the memory game after using {0} chances", trials - chances);
                return false;
            }
            if (chances == 0)
            {
                Console.WriteLine("\nYou have lost! :("); 
                return false;
            }
            return true;            
        }

        private int findIndex(int number)
        {
            int index = 0;
            foreach (var item in positionIndex)
            {
                if (item == number)
                    return index;
                index++;
            }
            return -1;
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
                    if (Duplicate(number, list.ToArray()))
                        str = randomizedWords[number];
                    else
                        str = number.ToString();
                    Console.Write("|");
                    for (int i = 0; i < (lenght - str.Length) / 2; i++)
                        Console.Write(" ");
                    Console.Write(str);
                    for (int i = 0; i < (lenght - str.Length + 1) / 2; i++)
                        Console.Write(" ");
                    number++;
                }
                Console.Write("|\n");
                PrintLineVertical();
            }

            PrintLineHorizontal();
            Console.WriteLine("\nGuess chances: " + chances.ToString());
            Console.WriteLine("Difficulty level: " + difficulty);
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

        private int[] RandomNoDuplicate(int amount, int number)
        {
            int[] arrey = new int[amount];
            Random rand = new Random();
            int tmp;

            for (int i = 0; i < amount; i++)
            {
                do
                {
                    tmp = rand.Next(number + 1);
                }while (Duplicate(tmp, arrey));
                arrey[i] = tmp;
            }
            return arrey;
        }

        private bool Duplicate(int tmp, int[] arrey)
        {
            foreach (var item in arrey)
            {
                if (item == tmp)
                    return true;
            }
            return false;
        }

        private int height, width, lenght, chances, trials;
        private string difficulty;
        private string[] randomizedWords;
        private int[] positionIndex;
        private List<int> list = new List<int>();
        private Stopwatch sw = new Stopwatch();

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
            int guess = -1;

            do
            {
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
                            Console.WriteLine("\nChoose one of the available options...");
                            break;
                    }
                } while (difficulty == 0);

                Matrix Memory = new Matrix(difficulty, lines);
    
                do
                {
                    if (!Int32.TryParse(Console.ReadLine(), out guess))
                        continue;
                } while (Memory.Play(guess));

                Console.WriteLine("\nPress \"Y\" to play again...");
            } while (Console.ReadKey().Key == ConsoleKey.Y);          
        }
    }
}
