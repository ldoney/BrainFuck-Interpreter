using System;
using System.Collections.Generic;
using System.Text;

namespace BrainFuck_Interpreter
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
//            AddLine(0, 1,       "char *d;");
//            AddLine(0, 2,       "const char *p;");
            AddLine(0, 1,       "int main(){");
            AddLine(1, 1,       "char data[30000] = {};");
            AddLine(1, 1,           "int i = 0;");
            AddLine(0, 0,           interpret());  
            AddLine(0, 1,       "}");
        }
        public string interpret()
        {
            int inc = 0;
            string res = "";
            string vardec = "";
            for (int i = 0; i < bfcode.Length; i++)
            {
                switch (bfcode[i])
                {
                    case '+':
                        int plu = Extensions.getPlus(i, bfcode);
                        if(plu != 1)
                        {
                            res += MakeLine(3 + inc, 1, "data[i] += " + Extensions.getPlus(i, bfcode) + ";");
                        }else
                        {
                            res += MakeLine(3 + inc, 1, "data[i]++;");
                        }
                        i += plu - 1; 
                        break;
                    case '-':
                        int min = Extensions.getPlus(i, bfcode);
                        if(min != 1)
                        {
                            res += MakeLine(3 + inc, 1, "data[i] -= " + Extensions.getPlus(i, bfcode) + ";");
                        }
                        else
                        {
                            res += MakeLine(3 + inc, 1, "data[i]" + "--;");
                        }
                        i += min - 1;
                        break;
                    case '>':
                        int rig = Extensions.getPlus(i, bfcode);
                        if (rig != 1)
                        {
                            res += MakeLine(3 + inc, 1, "i += " + Extensions.getPlus(i, bfcode) + ";");
                        }
                        else
                        {
                            res += MakeLine(3 + inc, 1, "i++;");
                        }
                        i += rig - 1;
                        break;
                    case '<':
                        int lef = Extensions.getPlus(i, bfcode);
                        if (lef != 1)
                        {
                            res += MakeLine(3 + inc, 1, "i -= " + Extensions.getPlus(i, bfcode) + ";");
                        }
                        else
                        {
                            res += MakeLine(3 + inc, 1, "i--;");
                        }
                        i += lef - 1;
                        break;
                    //if the byte at the data pointer is zero, then instead of moving the instruction pointer forward to the next command, jump it forward to the command after the matching ] command.
                    case '[':
                        res += MakeLine(3 + inc, 1, "while(data[i] != 0)");
                        res += MakeLine(3 + inc, 1, "{");
                        inc++;
                        break;
                        // if ptr != 0 move pointer >
                        // if ptr == 0 go back
                    //	if the byte at the data pointer is nonzero, then instead of moving the instruction pointer forward to the next command, jump it back to the command after the matching [ command.
                    case ']':
//                        res += MakeLine(3 + inc, 1, "if(data[i] != 0)");
//                        res += MakeLine(3 + inc, 1, "{");
//                        res += MakeLine(3 + inc, 1, "break;");
//                        res += MakeLine(3 + inc, 1, "}");
                        inc--;
                        res += MakeLine(3 + inc, 1, "};");
                        break;
                    case ',':
                        res += MakeLine(3 + inc, 1, "cin >>" + "data[i]" + ";");
                        break;
                    case '.':
                        res += MakeLine(3 + inc, 1, "cout << (char)" + "data[i]" + ";");
                        break;
                    default:
                        break;
                }
            }
//            for(int i = 0; i < vars.Count; i++)
//            {
//                vardec += MakeLine(3, 1, "int " + vars[i] + " = 0;");
//            }
            return vardec + res;
        }
        public void AddLine(int indent, string text)
        {
            AddLine(indent, 1, text);
        }
        public void AddLine(int indent, int nl, string text)
        {
            
            program += MakeLine(indent, nl, text);
        }
        public string MakeLine(int indent, int nl, string text)
        {
            string toAdd = "";
            for (int i = 0; i < indent; i++)
            {
                toAdd += "\t";
            }
            toAdd += text;
            for (int i = 0; i < nl; i++)
            {
                toAdd += "\n";
            }
            return toAdd;
        }
    }
}
