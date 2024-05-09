using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver.Common
{
    public class Common
    {
        public static (string, int) ParseFuncName(string command)
        {

            string funcName = string.Empty;
            int currentIndex = 0;
            do
            {
                funcName += command[currentIndex];
                currentIndex++;
            } while (command[currentIndex] != '(');

            return (funcName, currentIndex);
        }



        public static (List<string> parametersList, int currentIndex) ParseParameters(string command, int currentIndex)
        {
            List<string> parametersList = new();

            string currentParameter = string.Empty;
            do
            {
                if (command[currentIndex] == '(' || Char.IsWhiteSpace(command[currentIndex]))
                {
                    currentIndex++;
                    continue;
                }

                if (command[currentIndex] == ',')
                {
                    parametersList.Add(currentParameter);
                    currentIndex++;
                    currentParameter = string.Empty;
                    continue;
                }

                currentParameter += command[currentIndex];
                currentIndex++;

            } while (command[currentIndex] != ')');

            parametersList.Add(currentParameter);

            return (parametersList, currentIndex);
        }

    }

}
