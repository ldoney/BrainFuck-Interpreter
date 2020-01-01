using System;
using System.Collections.Generic;
using System.Text;

namespace BrainFuck_Compiler
{
    class BrainFuck
    {
        private string program = "";
        private string bfcode;
        public string asString()
        {
            return program;
        }
        public BrainFuck(string input)
        {
            bfcode = input;
            AddLine(0, 2, "#include <iostream>");
            AddLine(0, 2, "using namespace std;");
            AddLine(0, 1, "class Program{");
            AddLine(1, 1,   "public:");
            AddLine(2, 1,       "char data[30000]");
            AddLine(2, 1,       "char *d"); 
            AddLine(2, 2,       "const char *p");
            AddLine(2, 1,       "Program(){");
            AddLine(3, 1,           interpret());  
            AddLine(2, 1,       "}");
            AddLine(0, 1, "}");
        }
        public string interpret()
        {
            List<int> traversed = new List<int>();
            traversed.Add(0);
            List<string> vars = new List<string>();
            vars.Add("A");
            int curvar = 0;
            string res = "";
            for (int i = 0; i < bfcode.Length; i++)
            {
                switch (bfcode[i])
                {
                    case '+':
                        res += vars[curvar] + "++;\n";
                        break;
                    case '-':
                        res += vars[curvar] + "--;\n";
                        break;
                    case '>':
                        curvar++;
                        if(curvar > traversed[traversed.Count - 1])
                        {
                            traversed.Add(curvar);
                            vars.Add(Extensions.getPermutations(curvar));
                            res += "int " + vars[curvar] + "= 0;\n";
                        }
                        
                        break;
                    case '<':
                        break;
                    case '[':
                        break;
                    case ']':
                        break;
                    case ',':
                        break;
                    case '.':
                        break;
                    default:
                        break;
                }
            }
            return res;
        }
        public void AddLine(int indent, string text)
        {
            AddLine(indent, 1, text);
        }
        public void AddLine(int indent, int nl, string text)
        {
            string toAdd = "";
            for(int i = 0; i < indent; i++)
            {
                toAdd += "\t";
            }
            toAdd += text;
            for(int i = 0; i < nl; i++)
            {
                toAdd += "\n";
            }
            program += toAdd;
        }
    }
}
