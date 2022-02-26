using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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
            red = false;

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
                        red = true;
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
                        red = true;
                        Print();
                        list.RemoveAt(listLenght - 1);
                        list.RemoveAt(listLenght - 2);         
                    }
                }
            }
            if (list.Count == width * height)
            {
                sw.Stop();
                string time = sw.Elapsed.ToString();
                int count = trials - chances;
                Console.WriteLine("\nYou won! :)");  
                Console.WriteLine("Your time was " +  time);
                Console.WriteLine("You solved the memory game after using {0} chances", count);
                save(time, count);
                return false;
            }
            if (chances == 0)
            {
                Console.WriteLine("\nYou have lost! :(");
                top();
                return false;
            }
            red = false;
            return true;            
        }

        private void save(string time, int tries)
        {
            Console.Write("\nEnter your name: ");
            var dateString = DateTime.Now.ToString("yyyy-MM-dd");
            string text = time + " | " + Console.ReadLine() + " | " + dateString + " | " + tries.ToString();

            using (StreamWriter sw = File.AppendText("Score.txt"))
            {
                sw.WriteLine(text);
            }
            
            try
            {
                using (StreamReader sr = new StreamReader("Score.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        scores.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }       
            scores.Sort();

            using (StreamWriter sw = new StreamWriter("Score.txt"))
            {
                foreach (var item in scores)
                {
                    sw.WriteLine(item);
                }
            }
            if (scores.Count >= 10)
            {
                Console.WriteLine("\nTop 10 high scores:");
                for (int i = 0; i < 10; i++)
                    Console.WriteLine(scores[i]);
            }
            else
            { 
                Console.WriteLine("\nHigh scores:");
                foreach (var item in scores)
                    Console.WriteLine(item);
            }
        }

        private void top ()
        {
            try
            {
                using (StreamReader sr = new StreamReader("Score.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        scores.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            if (scores.Count >= 10)
            {
                Console.WriteLine("\nTop 10 high scores:");
                for (int i = 0; i < 10; i++)
                    Console.WriteLine(scores[i]);
            }
            else
            {
                Console.WriteLine("\nHigh scores:");
                foreach (var item in scores)
                    Console.WriteLine(item);
            }
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
            int listLenght = list.Count;
            string str;
            bool color = false;
            colors('m');
            Console.Clear();

            for (int k = 0; k < height; k++)
            {
                PrintLineHorizontal();
                PrintLineVertical();

                for (int j = 0; j < width; j++)
                {
                    if (Duplicate(number, list.ToArray()))
                    {
                        str = randomizedWords[number];
                        color = true;
                    }      
                    else
                    {
                        str = number.ToString();
                        color = false;
                    }         
                    colors('1');
                    Console.Write("|");
                    for (int i = 0; i < (lenght - str.Length) / 2; i++)
                        Console.Write(" ");
                    if (color)
                    {
                        colors('2');
                        if (listLenght != 0 && listLenght % 2 == 0)
                        {
                            if (!red)
                                colors('6');
                            else
                            {
                                colors('6');
                                if (number == list[listLenght - 2] || number == list[listLenght - 1])
                                    colors('4');
                            }
                        }
                        else if (listLenght > 2)
                        {
                            if (number != list[listLenght - 1])
                                colors('6');
                        }
                    }
                    else
                        colors('w');
                    Console.Write(str);
                    for (int i = 0; i < (lenght - str.Length + 1) / 2; i++)
                        Console.Write(" ");
                    number++;
                }
                colors('1');
                Console.Write("|\n");
                PrintLineVertical();
            }

            PrintLineHorizontal();
            colors('1');
            Console.Write("\nGuess chances: ");
            colors('5');
            Console.WriteLine(chances.ToString());
            colors('1');
            Console.Write("Difficulty level: ");
            colors('7');
            Console.WriteLine(difficulty + "\n");
            colors('1');
        }

        private void PrintLineHorizontal()
        {
            colors('1');
            Console.Write(" ");
            for (int i = 0; i < lenght * width + width - 1; i++)
                Console.Write("-");
            Console.Write("\n");
        }

        private void PrintLineVertical()
        {
            colors('1');
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
        private bool red;
        private string[] randomizedWords;
        private int[] positionIndex;
        private List<int> list = new List<int>();
        private List<string> scores = new List<string>();
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

        static void colors(char zmienna)
        {
            switch (zmienna)
            {
                case '1':
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case '2':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case '3':
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case '4':
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case '5':
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case '6':
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case '7':
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case '8':
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case 'q':
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 'w':
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case 'e':
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 'r':
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 't':
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case 'y':
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case 'u':
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 'i':
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 'a':
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case 's':
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case 'd':
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case 'f':
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case 'g':
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case 'h':
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case 'j':
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case 'k':
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case 'z':
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case 'x':
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case 'c':
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 'v':
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case 'b':
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case 'n':
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case 'm':
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case ',':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
            }
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
