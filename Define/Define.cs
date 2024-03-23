using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver.Define
{
    public class Define
    {
        public static (string, List<string>, string) Parse(string command)
        {
            string funcName = string.Empty;
            List<string> parametersList = new();
            string funcBody = string.Empty;
            int currentIndex = 0;
            do
            {
                funcName += command[currentIndex];
                currentIndex++;
            } while (command[currentIndex] != '(');

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
            return (funcName, parametersList, funcBody);
        }
        public static void BuildTree(string funcBody)
        {
            List<string> funcBodyElements = ParseFunctionBody(funcBody);
        }

        private static List<string> ParseFunctionBody(string funcBody)
        {
            List<string> funcBodyElements = new();
            bool isFunctionParametersList = false;
            string currentElement = string.Empty;

            foreach (var ch in funcBody) 
            {
                if (ch == '(')
                {
                    isFunctionParametersList = true;
                    currentElement += ch;
                    continue;
                }

                if (ch == ')') 
                {
                    isFunctionParametersList = false;
                    currentElement += ch;
                    continue;
                }

                if (Char.IsWhiteSpace(ch))
                {
                    if (isFunctionParametersList)
                    {
                        currentElement += ch;
                    }
                    else
                    {
                        funcBodyElements.Add(currentElement);
                        currentElement = string.Empty;
                    }

                    continue;
                }

                currentElement += ch;
            }

            if (!string.IsNullOrEmpty(currentElement))
            {
                funcBodyElements.Add(currentElement);
            }

            return funcBodyElements;
        }

    }
}

