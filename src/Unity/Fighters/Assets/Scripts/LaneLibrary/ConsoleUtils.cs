using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;

namespace LaneLibrary
{
    public static class ConsoleUtils
    {

        public static void WaitForKeyPress()
        {
            PrintSameLine("[Press any key to continue...]");
            ReadKey();
            Print();
        }
        public static void WaitForKeyPress(bool ClearLineAfter)
        {
            PrintSameLine("[Press any key to continue...] ");
            ReadKey();
            // I like to clear the line after and then leave the space so my console isnt just filled with
            // waiting for key press messages.
            if (ClearLineAfter) 
            {
                //GoBackOneLine();
                ClearCurrentConsoleLine();
                
            }
            Print();
        }

        // i had methods like these in one of my projects last semester so i looked up how to do this again
        public static void GoBackOneLine()
        {
            
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            
        }
        public static void ClearCurrentConsoleLine()
        {
            //from stackoverflow https://stackoverflow.com/questions/8946808/can-console-clear-be-used-to-only-clear-a-line-instead-of-whole-console
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void Print()
        {
            WriteLine();
        }
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void PrintSameLine(string message)
        {
            Console.Write(message);
        }

        public static bool GetInputBool()
        {
            PrintSameLine("[Y/N]: ");

            
            string input = ReadKey(false).KeyChar.ToString();
            Print();

            //sneakily allows for 1 or 2 to be used as yes or no, so you can play without moving your hand

            if (input == "Y" || input == "y" || input == "1")
            {
                return true;
            }
            else if (input == "N" || input == "n" || input == "2")
            {
                return false;
            }
            else
            {
                Print("[Error, you must input \"Y\" or \"N\"]");
                return GetInputBool();
            }
        }

        
        public static int GetInputInt(int min, int max)
        {
            PrintSameLine("[" + min.ToString() + " - " + max.ToString() + "]: ");
            string input = ReadLine();
            int inputInt;
            try
            {
                inputInt = int.Parse(input);
                if (inputInt >= min && inputInt <= max)
                {
                    return inputInt;
                }
                else
                {
                    Print("[Error, you must input an integer between the Minimum and Maximum values]");
                    return GetInputInt(min, max);
                }
            }
            catch (Exception)
            {
                Print("[Error, you must input an integer between the Minimum and Maximum values]");
                return GetInputInt(min, max);
            }
            
        }

        public static int GetInputIntKey(int min, int max)
        {
            //gets int input without pressing enter. will need to be between 0-9
            PrintSameLine("[" + min.ToString() + " - " + max.ToString() + "]: ");


            string input = ReadKey(false).KeyChar.ToString();
            Print();
            int inputInt;
            try
            {
                inputInt = int.Parse(input);
                if (inputInt >= min && inputInt <= max)
                {
                    return inputInt;
                }
                else
                {
                    Print("[Error, you must input an integer between the Minimum and Maximum values]");
                    return GetInputIntKey(min, max);
                }
            }
            catch (Exception)
            {
                Print("[Error, you must input an integer between the Minimum and Maximum values]");
                return GetInputIntKey(min, max);
            }

        }


        public static string LoadTextFromFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static void ClearScrollable()
        {
            int height = Console.WindowHeight;
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            
            
            for (int i = 0; i < WindowHeight; i++)
            {
                Print();
            }

            Console.SetCursorPosition(0, currentLineCursor+1);
            
        }

    }
}
