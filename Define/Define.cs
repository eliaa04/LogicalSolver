using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver.Define
{
    public class Define
    {
       
        public static string Parse(string command, int currentIndex)
        {
            string funcBody = string.Empty;
            for (int i = currentIndex; i < command.Length; i++)
            {
                if (command[i] == '"')
                {
                    for (int j = i + 1; j < command.Length - 1; j++)
                    {
                        funcBody += command[j];
                    }
                }
            }
            return ( funcBody);
        }
    }
}

