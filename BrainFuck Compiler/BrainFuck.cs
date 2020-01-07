using System;
using System.Collections.Generic;
using System.Text;

namespace BrainFuck_Interpreter
{
    class BrainFuck
    {
        //The actual c++ code, start empty
        private string program = "";
        
        //The Brain Fuck code itself
        private readonly string bfcode;

        //Print out the generated c++ code
        public string AsString()
        {
            return program;
        }

        //Constructor, takes an input of BrainFuck code as a string
        public BrainFuck(string input)
        {
            //Set the bfcode variable to be
            bfcode = input;

            //C++ setup
            AddLine(0, 2, "#include <iostream>");
            AddLine(0, 2, "using namespace std;");
            AddLine(0, 1,       "int main(){");
            //All of the variables needed for the c++ code
            AddLine(1, 1,           "char data[30000] = {};");
            AddLine(1, 1,           "int i = 0;");
            AddLine(0, 0,           Interpret());  

            AddLine(0, 1,       "}");
        }

        //Interpret the actual code, return as a string
        public string Interpret()
        {
            //Increments to make the while loops looks pretty
            int inc = 0;

            //Result, start empty
            string res = "";

            //Loop through all of the BrainFuck code
            for (int i = 0; i < bfcode.Length; i++)
            {
                //Find out what the code represents
                switch (bfcode[i])
                {
                    //If its a +, generate code to append
                    case '+':
                        //Find the # of +s in a row to make the generated code cleaner
                        int plu = Extensions.GetPlus(i, bfcode);

                        //If there is more than one plus, do += instead to beautify it
                        if(plu != 1)
                        {
                            res += MakeLine(3 + inc, 1, "data[i] += " + plu + ";");
                        }

                        //If there is only one plus, use ++ because it looks ugly to do += 1
                        else
                        {
                            res += MakeLine(3 + inc, 1, "data[i]++;");
                        }

                        //Increment the instruction to keep the code consistent
                        i += plu - 1; 
                        break;

                    //If its a -, generate code to decrement
                    case '-':
                        //Find the # of -s in a row to make the generated code cleaner
                        int min = Extensions.GetPlus(i, bfcode);

                        //If there is more than one minus, do -= instead to beautify it
                        if (min != 1)
                        {
                            res += MakeLine(3 + inc, 1, "data[i] -= " + min + ";");
                        }

                        //If there is only one minus, use -- because it looks ugly to do -= 1
                        else
                        {
                            res += MakeLine(3 + inc, 1, "data[i]" + "--;");
                        }

                        //Increment the instruction to keep the code consistent
                        i += min - 1;
                        break;

                    //Given the increment pointer instruction, generate the code for increasing the pointer
                    case '>':

                        //Find the amount of >s in a row
                        int rig = Extensions.GetPlus(i, bfcode);

                        //If there is more than one >, do += instead to beautify it
                        if (rig != 1)
                        {
                            res += MakeLine(3 + inc, 1, "i += " + rig + ";");
                        }

                        //If there is only one >, use ++ because it looks ugly to do += 1
                        else
                        {
                            res += MakeLine(3 + inc, 1, "i++;");
                        }

                        //Increment the instruction to keep the code consistent
                        i += rig - 1;
                        break;
                    case '<':

                        //Find the # of <s in a row to make the generated code cleaner
                        int lef = Extensions.GetPlus(i, bfcode);

                        //If there is more than one minus, do -= instead to beautify it
                        if (lef != 1)
                        {
                            res += MakeLine(3 + inc, 1, "i -= " + lef + ";");
                        }

                        //If there is only one minus, use -- because it looks ugly to do -= 1
                        else
                        {
                            res += MakeLine(3 + inc, 1, "i--;");
                        }

                        //Increment the instruction to keep the code consistent
                        i += lef - 1;
                        break;

                    //Essentially generating a while loop
                    case '[':
                        res += MakeLine(3 + inc, 1, "while(data[i] != 0)");
                        res += MakeLine(3 + inc, 1, "{");
                        inc++;
                        break;

                    //Generating the end of a while loop
                    case ']':
                        inc--;
                        res += MakeLine(3 + inc, 1, "};");
                        break;

                    //Generate a read line command
                    case ',':
                        res += MakeLine(3 + inc, 1, "cin >>" + "data[i]" + ";");
                        break;

                    //Generate a command to print what's being pointed to
                    case '.':
                        res += MakeLine(3 + inc, 1, "cout << (char)" + "data[i]" + ";");
                        break;

                    //Should never be called
                    default:
                        break;
                }
            }
            return res;
        }
        //This is to just standardize the code, this just adds to the code so it looks prettier up top
        public void AddLine(int indent, int nl, string text)
        {
            //Call MakeLine() as it performs the same function
            program += MakeLine(indent, nl, text);
        }

        //More broad definition, it generates code with the amount of tabs and indentations to make
        //generated code look pretty
        public string MakeLine(int indent, int nl, string text)
        {
            //Create empty string
            string toAdd = "";

            //Add amount of tabs
            for (int i = 0; i < indent; i++)
            {
                toAdd += "\t";
            }

            //Append on the text
            toAdd += text;

            //Append the amount of newlines
            for (int i = 0; i < nl; i++)
            {
                toAdd += "\n";
            }
            return toAdd;
        }
    }
}
