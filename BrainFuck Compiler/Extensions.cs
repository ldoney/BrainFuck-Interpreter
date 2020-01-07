using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrainFuck_Interpreter
{
    //The extenstions to the interpreter that are used programatically that technically have no
    //use solely for my code
    static class Extensions
    {
        //I dont think I ever used this but its like substring for an array
        public static T[] Subsequence<T>(this IEnumerable<T> arr, int startIndex, int length)
        {
            return arr.Skip(startIndex).Take(length).ToArray();
        }

        //Finds all indexes of a value in an array.. indexOf on steroids
        public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
        {
            return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        }

        //Find the most recent BALANCED instance of a [] within a string at an index
        public static int FindLast(String expression, int index)
        {
            //Take the expression and convert it to an array to make it easier
            char[] arr = expression.ToCharArray();

            //Create a stack of chars for the algorithm
            Stack<char> stack = new Stack<char>();

            //Move backwards in the char array, starting at the index
            for (int i = index; i >= 0; i--)
            {
                //If the item is ], add to the stack
                if (arr[i] == ']')
                { stack.Push(arr[i]); }

                //If the item is [, remove from the stack
                if (arr[i] == '[')
                { stack.Pop(); }

                //If there is nothing in the stack, it has been found
                if (stack.Count == 0)
                { return i; }
            }

            //If nothing can be found, return -1
            return -1;
        }

        //Find the next BALANCED instance of [] within a string
        public static int FindNext(String expression, int index)
        {
            //Take the expression and convert it to an array to make it easier
            char[] arr = expression.ToCharArray();

            //Create a stack of chars for the algorithm
            Stack<char> stack = new Stack<char>();

            //Start at zero and move to the end of the array
            for (int i = index; i < arr.Length; i++)
            {
                //If its [, append to the stack
                if (arr[i] == '[')
                { stack.Push(arr[i]); }
                
                //if its ], append to the stack
                if (arr[i] == ']')
                { stack.Pop(); }

                //If the stack is empty, the index has been found and return it
                if (stack.Count == 0)
                { return i; }
            }

            //If nothing has been found, return -1
            return -1;
        }

        //Used if you want to recieve the file
        public static string GetFile(string input)
        {
            //Simply read the file, if it can't be found then throw an error
            if (File.Exists(input))
            {
                return File.ReadAllText(input);
            }
            else
            {
                throw new IOException();
            }
        }
        
        //Clear the console line, just makes stuff clean
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        
        //Get the amount of a string after an index repeated
        public static int GetPlus(int index, string str)
        {
            //Convert the input to a char array
            char[] charArray = str.ToCharArray();

            //Find the initial string
            char toFind = charArray[index];

            //Result starts at zero
            int res = 0;

            //The current string is toFind at the moment to keep the algorithm consistent
            char current = toFind;

            //Keep on looping until it hits another char
            while(true)
            {

                //Keep on moving through the array
                toFind = charArray[index + res];

                //If it is a different char, exit out of the loop
                if(toFind != current)
                {
                    break;
                }

                //Increment the result
                res++;
            }
            return res;
        }
    }
}
