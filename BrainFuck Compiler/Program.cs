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
        //Use textreader if a file is to be read (Practically unused)
        static TextReader textreader = Console.In;

        //List of keywords, really just a reassurance when reading the keywords
        readonly static List<char> KeyWords = new List<char>() { '+', '-', '<', '>', '.', ',', '[', ']' };

        //The size for the stack's display
        static int STACK_DIST = 10;

        //Set the time for the program to sleep between steps 
        static int SLEEP_TIME = 100;

        static void Main(string[] args)
        {
            //This will allow me to debug if I need to test a method so I dont have to comment the whole code out
            try
            {
                Run(args);
            }catch(Exception e)
            {
                //In case there are any sort of exceptions to clean up the ugliness of a normal error
                Console.WriteLine("Unexpected error!");
            }
        }
        static void Run(string[] args)
        {
            //If there were no arguments passed, stop the progrma
            if (!args.Any())
            {
                Console.WriteLine("You must enter args!");
                Console.WriteLine("Type help to recieve commands");
                return;
            }

            //Get the instruction which the user wants to perform
            //from: Converts BrainFuck code to c++ code
            //to: (UNUSED), converts c++ code to BrainFuck
            //run: Runs a bit of BrainFuck code
            //step: Runs BrainFuck code and shows a display of what's happening
            //help: Shows commands
            string inst = args[0];

            //The code/File to use 
            string input = "";

            if (inst.ToLower() == "from")
            {
                //Convert BrainFuck to C++

                //If the file exists, read it and make it input
                try
                {
                    //Get the file relative to the run path and read its contents
                    string content = Extensions.GetFile(args[1]);
                    input = content;
                }
                //If the file doesn't exist, make the input be the string passed
                catch (IOException e)
                {
                    input = args[1];
                }

                //Create new BrainFuck object based on the input
                BrainFuck bf = new BrainFuck(input);
                
                //Print out the c++ version of the program
                Console.Write(bf.AsString());
            }
            else if (inst.ToLower() == "to")
            {
                //Convert C++ to BrainFuck
                //TODO: Implement this feature

                //Only if the file is a cpp file, use it
                if (args[1].EndsWith("cpp"))
                {
                    //If the file can be found, use it
                    try
                    {
                        string content = Extensions.GetFile(args[1]);
                        input = content;
                    }
                    //If the file can't be found, don't use it
                    catch (IOException e)
                    {
                        input = args[1];
                    }
                }
                //A double check to input the c++ code
                else
                {
                    input = args[1];
                }
            }else if(inst.ToLower() == "step" || inst.ToLower() == "run")
            {
                //If they just want to run their brainfuck code
                
                //Just to clear up some of the code and make it prettier
                Boolean step = false;
                if(inst.ToLower() == "step")
                {
                    step = true;
                    //Clear the console to make the output pretty
                    Console.Clear();
                }

                //Part of the output, reads out how many instructions have been completed
                int instructions = 0;

                //Prints out the final string
                string fin = "";

                //Convert the BF code into a char array
                input = args[1];
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

                                    //If there is no ] then throw an error
                                    if (next == -1)
                                    {
                                        throw new CompileError("Could not find next ]", stack[cursor]);
                                    }

                                    //Append i to the next ]
                                    i = next;
                                }
                                break;
                            //	if the byte at the data pointer is nonzero, then instead of moving the instruction pointer forward to the next command, jump it back to the command after the matching [ command.
                            case ']':
                                if (stack[cursor] != 0)
                                {
                                    //Get index of last [
                                    int last = Extensions.FindLast(new string(chinpt), i);
                                    
                                    //If there is no [ then throw an error
                                    if (last == -1)
                                    {
                                        throw new CompileError("Could not find last [", stack[cursor]);
                                    }

                                    //Append i to the next [
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
                                if(!step)
                                {
                                    Console.Write((char)stack[cursor]);
                                }
                                else
                                {
                                    fin += ((char)stack[cursor]);
                                }
                                break;
                            default:
                                break;
                        }
                        if(step)
                        {
                            //Sleep for SLEEP_TIME time
                            Thread.Sleep(SLEEP_TIME);

                            //Clear the first line for the stack printout
                            Extensions.ClearCurrentConsoleLine();

                            //Print out the stack delimetered by \t to keep it equidistant
                            for (int j = 0; j < STACK_DIST; j++)
                            {
                                Console.Write(stack[j] + "\t");
                                if(j == STACK_DIST - 1)
                                {
                                    Console.Write("\n");
                                }
                            }

                            //Clear the second line for the pointer printout
                            Extensions.ClearCurrentConsoleLine();

                            //Print out the character representation of the string
                            for (int j = 0; j < STACK_DIST; j++)
                            {
                                //Only print out actual ASCII characters (Some ASCII characters mess 
                                //with newlines and tabs so they're a pain to work with)
                                if(stack[j] >= 32 && stack[j] <= 126)
                                {
                                    Console.Write(((char)stack[j]));
                                }
                                Console.Write("\t");
                                if(j == STACK_DIST - 1)
                                {
                                    Console.Write("\n");
                                }
                            }

                            //Clear the pointer line
                            Extensions.ClearCurrentConsoleLine();
                            
                            //The amount of spaces
                            string spac = "";
                            for (int j = 0; j < cursor; j++)
                            {
                                spac += "\t";
                            }

                            //Append ^ to the end of the spaces
                            spac += "^";

                            //Print out the spaces and a new line
                            Console.WriteLine(spac + "\n");

                            //Print out the position in the code
                            Extensions.ClearCurrentConsoleLine();
                            Console.WriteLine("Currently at position: " + i);

                            //Print out the keyword it is currently executing
                            Extensions.ClearCurrentConsoleLine();
                            Console.WriteLine("Executing: " + kw);

                            //Increment the amount of instructions and print it out
                            instructions++;
                            Extensions.ClearCurrentConsoleLine();
                            Console.WriteLine("i: " + instructions);

                            //Print out the value of the point in the stack it is in
                            Extensions.ClearCurrentConsoleLine();
                            Console.WriteLine("Pointing to: " + stack[cursor]);

                            //Print out the index of the stack where the pointer is
                            Extensions.ClearCurrentConsoleLine();
                            Console.WriteLine("Pointer: " + cursor);

                            //Print the current output
                            Extensions.ClearCurrentConsoleLine();
                            Console.WriteLine("Output: " + fin + "\n");

                            //Reset the console
                            Console.SetCursorPosition(0,0);
                        }
                    }
                }
            }
            else if(inst.ToLower() == "help")
            {
                //Printout for help (UNUSED AND UNFINISHED)
                //TODO: Add in the implementation
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
                //If they have an unknown instruction, tell them
                Console.Write("Invalid args!");
            }
        }

        //Create a BrainFuck compilation error that I can throw if I want
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
