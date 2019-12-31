using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrainFuck_Compiler
{
    class Program
    {
        static TextReader textreader = Console.In;
        readonly static List<char> KeyWords = new List<char>() { '+', '-', '<', '>', '.', ',', '[', ']' };
        static void Main(string[] args)
        {
            Console.WriteLine(Extensions.FindNext("Hellllo".ToCharArray(), 0, "l", 4));
            /*
            string inst = args[0];
            string input = "";
            try
            {
                string content = Extensions.GetFile(args[1]);
                //C# code
                if(args[1].EndsWith("cs"))
                {
                    
                }
                //BrainFuck code
                else
                {
                    input = content;
                }
            }catch(IOException e)
            {
                input = args[1];
            }
            
            if(inst.ToLower() == "from")
            {
                //Convert BrainFuck to C#
                //TODO: Implement this feature
            }else if(inst.ToLower() == "to")
            {
                //Convert C# to BrainFuck
                //TODO: Implemenet this feature
            }
            else if(inst.ToLower() == "run")
            {
                unsafe
                {
                    //If they just want to run their brainfuck code
                    //Convert the BF code into a char array
                    var chinpt = input.ToCharArray();
                    //Define the stack (I doubt anyone's going over 30,000 addresses)
                    byte[] stack = new byte[30000];
                    //Define the entry point of the stack to the first address
                    int nestlevel = 0;
                    fixed(byte* ptr = &stack[0])
                    {
                        //Create a temporary cursor to navigate the stack
                        byte* cursor = ptr;
                        //Loop through the input code
                        for (int i = 0; i < chinpt.Length; i++)
                        {
                            Console.WriteLine(i + "\t" + (char)*ptr);
                            //Just to make it easier for me to debug
                            char kw = chinpt[i];
                            //Ensure that the code is not a comment or a space
                            if (KeyWords.Contains(kw))
                            {
                                //Easier to read than if statements
                                switch (kw)
                                {
                                    //Add
                                    case '+':
                                        //Increment the address the pointer points to
                                        ++*ptr;
                                        break;
                                    //Subtract
                                    case '-':
                                        //Subtract the address the pointer points to
                                        --*ptr;
                                        break;
                                    //Increment pointer
                                    case '>':
                                        //Increment pointer to right
                                        cursor++;
                                        break;
                                    //Decrement pointer
                                    case '<':
                                        //Decrement pointer to left
                                        cursor--;
                                        break;
                                    //if the byte at the data pointer is zero, then instead of moving the instruction pointer forward to the next command, jump it forward to the command after the matching ] command.
                                    case '[':
                                            if(*cursor == 0)
                                            {
                                                int next = Extensions.FindNext(chinpt, *cursor, "]", nestlevel);
                                                if (next == -1)
                                                {
                                                    throw new CompileError("Could not find next ]", *cursor);
                                                }
                                                
                                                i += next + 1;
                                            }else
                                            {
                                                nestlevel++;
                                            }
                                            break;
                                    //	if the byte at the data pointer is nonzero, then instead of moving the instruction pointer forward to the next command, jump it back to the command after the matching [ command.
                                    case ']':
                                           if(*cursor != 0)
                                           {
                                                int last = Extensions.FindPrev(chinpt, *cursor, "[", nestlevel);
                                                if(last == -1)
                                                {
                                                    throw new CompileError("Could not find last [", *cursor);
                                                }
                                                i -= last + 1;
                                           }else
                                           {
                                                //Move out of nest
                                                nestlevel--;
                                           }
                                        break;
                                    //Get user input
                                    case ',':
                                        try
                                        {
                                            //Get input and set it to the user's input
                                           *ptr = (byte)Console.ReadKey().KeyChar;
                                        }
                                        catch (InvalidOperationException e)
                                        {
                                            //If they enter an invalid character, throw an exception
                                            throw;
                                        }
                                        break;
                                    //Print out whats at the address
                                    case '.':
                                        //Write whats at the address
                                        Console.Write((char)*ptr);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            */
        }
        

    [Serializable]
    class CompileError : Exception
    {
        public CompileError()
        {

        }

        public CompileError(String reason, int index)
            : base(String.Format("ERROR: position {0} {1}", index, reason))
        {

        }

    }
    }
    public static class Extensions
    {
        public static T[] Subsequence<T>(this IEnumerable<T> arr, int startIndex, int length)
        {
            return arr.Skip(startIndex).Take(length).ToArray();
        }
        public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
        {
            return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        }
        public static int FindNext(char[] input, int from, string look, int nest)
        {

        }
        public static int FindNext(char[] input, int from, string look)
        {

        }
        public static int FindNext(string input, int from, string look)
        {
            return input.FindAllIndexof(look);
        }
        public static int FindPrev()
        {

        }
        /*
        //TODO: Improve this method... probably make it recursive
        public static int FindPrev(char[] input, int from, string look, int nest)
        {
            int fr = from;
            for (int i = 0; i < nest; i++)
            {
                fr = FindPrev(input.Subsequence(0, fr), fr, look);
            }
            return fr;
        }
        public static int FindPrev(char[] input, int from, string look)
        {
            string str = "";
            for (int i = 0; i < input.Length; i++)
            {
                str += input[i];
            }
            return FindPrev(str, from, look);
        }
        public static int FindPrev(string input, int from, string look)
        {
            return input.LastIndexOf(look, from);
        }

        public static int FindNext(char[] input, int from, string look, int nest)
        {
            int fr = from;
            int addend = 0;
            for (int i = 0; i < nest+1; i++)
            {
                input = input.Subsequence(fr, input.Length - fr + 1);
                fr = FindNext(input, 0, look);
                if(fr == 0)
                {
                    addend++;
                }else
                {
                    addend += fr;

                }
            }
            return addend;
        }
        public static int FindNext(char[] input, int from, string look)
        {
            string str = "";
            for (int i = 0; i < input.Length - from; i++)
            {
                str += input[i + from];
            }
            return FindNext(str, from, look);
        }
        public static int FindNext(string input, int from, string look)
        {
             return input.IndexOf(look, from);
        }
        */
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
    }
}
