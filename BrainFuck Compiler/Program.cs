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
            Console.WriteLine(Extensions.getPermutations(0));
            Run(args);
        }
        static void Run(string[] args)
        {
            string inst = args[0];
            string input = "";
            if (!args.Any()) { Console.WriteLine("You must enter args!");Console.WriteLine("Type help to recieve commands") ; return; }

            if (inst.ToLower() == "from")
            {
                //Convert BrainFuck to C++
                //TODO: Implement this feature
                try
                {
                    string content = Extensions.GetFile(args[1]);
                    input = content;
                }
                catch (IOException e)
                {
                    input = args[1];
                }
                BrainFuck bf = new BrainFuck(input);
                //Console.Write(bf.asString());
            }
            else if (inst.ToLower() == "to")
            {
                //Convert C++ to BrainFuck
                //TODO: Implement this feature
                if (args[1].EndsWith("cs"))
                {
                    try
                    {
                        string content = Extensions.GetFile(args[1]);
                        input = content;
                    }
                    catch (IOException e)
                    {
                        input = args[1];
                    }
                }
                else
                {
                    input = args[1];
                }
            }
            else if (inst.ToLower() == "run")
            {
                //If they just want to run their brainfuck code
                //Convert the BF code into a char array
                var chinpt = input.ToCharArray();
                //Define the stack (I doubt anyone's going over 30,000 addresses)
                byte[] stack = new byte[30000];
                //Define the entry point of the stack to the first address
                int cursor = 0;
                //Loop through the input code
                for (int i = 0; i < chinpt.Length; i++)
                {
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
                                stack[cursor]++;
                                break;
                            //Subtract
                            case '-':
                                //Subtract the address the pointer points to
                                stack[cursor]--;
                                break;
                            //Increment pointer
                            case '>':
                                //Increment pointer to right
                                if (!(cursor > stack.Length))
                                {
                                    cursor++;
                                }
                                break;
                            //Decrement pointer
                            case '<':
                                //Decrement pointer to left
                                if (!(cursor <= 0))
                                {
                                    cursor--;
                                }
                                break;
                            //if the byte at the data pointer is zero, then instead of moving the instruction pointer forward to the next command, jump it forward to the command after the matching ] command.
                            case '[':
                                if (stack[cursor] == 0)
                                {
                                    //Get index of next ]
                                    int next = Extensions.FindNext(new string(chinpt), i);
                                    if (next == -1)
                                    {
                                        throw new CompileError("Could not find next ]", stack[cursor]);
                                    }
                                    i = next;
                                }
                                break;
                            //	if the byte at the data pointer is nonzero, then instead of moving the instruction pointer forward to the next command, jump it back to the command after the matching [ command.
                            case ']':
                                if (stack[cursor] != 0)
                                {
                                    //Get index of last [
                                    int last = Extensions.FindLast(new string(chinpt), i);
                                    if (last == -1)
                                    {
                                        throw new CompileError("Could not find last [", stack[cursor]);
                                    }
                                    i = last;
                                }
                                break;
                            //Get user input
                            case ',':
                                try
                                {
                                    //Get input and set it to the user's input
                                    stack[cursor] = (byte)Console.ReadKey().KeyChar;
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
                                Console.Write((char)stack[cursor]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }else if(inst.ToLower() == "help")
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("from");
                Console.Write("From");
                Console.WriteLine("to");
                Console.Write("To");
                Console.WriteLine("run");
                Console.Write("Run");
            }
            else
            {
                Console.Write("Invalid args!");
            }
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
}
