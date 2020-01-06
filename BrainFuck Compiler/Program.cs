using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
/*
 * Created by: Lincoln Doney
 * Created on: 01/06/2020
 * Name: Brainfuck Interpreter
 * Purpose: Interprets BrainFuck code into more.. readable code
 * TODO:
 * - Add comments maybe
 * - C++ to BrainFuck...?
 */
namespace BrainFuck_Interpreter
{
    class Program
    {
        static TextReader textreader = Console.In;
        readonly static List<char> KeyWords = new List<char>() { '+', '-', '<', '>', '.', ',', '[', ']' };
        static void Main(string[] args)
        {
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
                Console.Write(bf.asString());
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
            }else if(inst.ToLower() == "step" || inst.ToLower() == "run")
            {
                Boolean step = false;
                if(inst.ToLower() == "step")
                {
                    step = true;
                }
                //If they just want to run their brainfuck code
                //Convert the BF code into a char array
                input = args[1];
                int instructions = 0;
                var chinpt = input.ToCharArray();
                //Define the stack (I doubt anyone's going over 30,000 addresses)
                byte[] stack = new byte[30000];
                //Define the entry point of the stack to the first address
                int cursor = 0;
                string fin = "";
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
                                fin += ((char)stack[cursor]);
                                break;
                            default:
                                break;
                        }
                        if(step)
                        {
                            int STACK_DIST = 10;

                            Thread.Sleep(1);
                            Extensions.ClearCurrentConsoleLine();
                            for (int j = 0; j < STACK_DIST; j++)
                            {
                                Console.Write(stack[j] + "\t");
                            }
                            Console.WriteLine("");
                            Extensions.ClearCurrentConsoleLine();
                            for (int j = 0; j < STACK_DIST; j++)
                            {
                                if(stack[j] >= 32 && stack[j] <= 126)
                                {
                                    Console.Write(((char)stack[j]));
                                }
                                Console.Write("\t");
                            }
                            Console.WriteLine("");
                            Extensions.ClearCurrentConsoleLine();
                            string spac = "";
                            for (int j = 0; j < cursor; j++)
                            {
                                spac += "\t";
                            }
                            spac += "^";
                            Console.WriteLine(spac + "\n");
                            Console.WriteLine("Currently at position: " + i);
                            Console.WriteLine("Executing: " + kw);
                            instructions++;
                            Console.WriteLine("i: " + instructions);
                            Console.WriteLine("Pointing to: " + stack[cursor]);
                            Console.WriteLine("Pointer: " + cursor);
                            Console.WriteLine("Output: " + fin + "\n");
                            Console.SetCursorPosition(0,0);
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
