﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrainFuck_Interpreter
{
    static class Extensions
    {
        public static T[] Subsequence<T>(this IEnumerable<T> arr, int startIndex, int length)
        {
            return arr.Skip(startIndex).Take(length).ToArray();
        }
        public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
        {
            return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        }
        public static int FindLast(String expression, int index)
        {
            char[] arr = expression.ToCharArray();
            Stack<char> stack = new Stack<char>();
            for (int i = index; i >= 0; i--)
            {
                if (arr[i] == ']')
                { stack.Push(arr[i]); }
                if (arr[i] == '[')
                { stack.Pop(); }
                if (stack.Count == 0)
                { return i; }
            }
            return -1;
        }
        public static int FindNext(String expression, int index)
        {
            char[] arr = expression.ToCharArray();
            Stack<char> stack = new Stack<char>();
            for (int i = index; i < arr.Length; i++)
            {
                if (arr[i] == '[')
                { stack.Push(arr[i]); }
                if (arr[i] == ']')
                { stack.Pop(); }
                if (stack.Count == 0)
                { return i; }
            }
            return -1;
        }
        //Used if you want to recieve the file

        public static string GetFile(string input)
        {
            var path = input;
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                throw new IOException();
            }
        }

        public static string getPermutations(int index)
        {
            string res = "";
            if (index < 26)
            {
                res += (char)(65 + index);
            }
            else
            {
                for (int i = 0; i <= index / 26; i++)
                {
                    res += ((char)(65 + ((index * i) % 26)));
                }
            }
            return res;
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public static int getPlus(int index, string str)
        {
            char[] charArray = str.ToCharArray();
            char toFind = charArray[index];
            int res = 0;
            char current = toFind;
            while(true)
            {
                toFind = charArray[index + res];
                if(toFind != current)
                {
                    break;
                }
                res++;
            }
            return res;
        }
    }
}
