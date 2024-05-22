using LogicalSolver;
using LogicalSolver.Common;
using LogicalSolver.Define;
using LogicalSolver.Solve;
using System.Linq.Expressions;
using System.Text;

namespace CourseProject
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Dictionary<string, TreeNode> rootByFuncNames = ReadDefinitions();
            Dictionary<string, List<string>> parametersByFuncNames = ReadParameters();
            Dictionary<string, bool> solvedFunctions = ReadSolutions();


            while (true)
            {
                try
                {
                    Console.WriteLine("Въведете команда:");
                    string command = Console.ReadLine();

                    string commandName = string.Empty;

                    foreach (var symbol in command)
                    {
                        if (Char.IsWhiteSpace(symbol))
                        {
                            break;
                        }

                        commandName += symbol;
                    }

                    var commandRemainder = Common.CustomSubstring(command,commandName.Length + 1);
                    string funcName = string.Empty;
                    int currentIndex = 0;

                    switch (commandName.ToUpperInvariant())
                    {
                        case "DEFINE":
                            (funcName, currentIndex) = Common.ParseFuncName(commandRemainder);
                            (List<string> parametersList, currentIndex) = Common.ParseParameters(commandRemainder, currentIndex);
                            string funcBody = Define.Parse(commandRemainder, currentIndex);
                            Tree tree = new Tree();
                            TreeNode root = tree.BuildTree(funcBody);
                            Define.ValidateDefinitionCandidate(root, parametersList, rootByFuncNames);
                            rootByFuncNames.Add(funcName, root);
                            parametersByFuncNames.Add(funcName, parametersList);
                            WriteDefinitions(funcName, funcBody);
                            WriteParameters(funcName, parametersList);
                            break;
                        case "SOLVE":
                            if (solvedFunctions.ContainsKey(commandRemainder))
                            {
                                Console.WriteLine(solvedFunctions[commandRemainder]);
                                break;
                            }
                            (funcName, currentIndex) = Common.ParseFuncName(commandRemainder);
                            (List<string> argumentsList, currentIndex) = Common.ParseParameters(commandRemainder, currentIndex);

                            TreeNode rootForSolving = null;

                            if (rootByFuncNames.ContainsKey(funcName))
                            {
                                rootForSolving = rootByFuncNames[funcName];
                            }

                            List<string> parametersForSolving = new();
                            if (parametersByFuncNames.ContainsKey(funcName))
                            {
                                parametersForSolving = parametersByFuncNames[funcName];
                            }

                            Solve.ReplaceParametersWithValues(rootForSolving, parametersForSolving, argumentsList, solvedFunctions,
                                rootByFuncNames);
                            bool treeResult = Solve.SolveNode(rootForSolving);
                            solvedFunctions.Add(commandRemainder, treeResult);
                            WriteSolutions(commandRemainder, treeResult);

                            Console.WriteLine(treeResult);
                            break;
                        case "ALL":
                            Console.WriteLine("bla bla all");
                            break;
                        default:
                            Console.WriteLine("Командата не е разпозната. Въведете валидна команда!");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            
        }

        private static void WriteDefinitions(string funcName, string funcBody)
        {
            File.AppendAllLines(@"..\..\..\Files\definitions.txt", new[] { $"{funcName}:{funcBody}" });
        }

        public static void WriteSolutions(string commandRemainder, bool treeResult)
        {
            File.AppendAllLines(@"..\..\..\Files\solutions.txt", new[] { $"{commandRemainder}:{treeResult}" });
        }

        private static void WriteParameters(string funcName, List<string> parametersList)
        {
            string parametersString = string.Empty;
            for (int i = 0; i < parametersList.Count; i++)
            {
                parametersString += parametersList[i];
                if (i != parametersList.Count - 1)
                {
                    parametersString += ",";
                }
            }

            File.AppendAllLines(@"..\..\..\Files\parameters.txt", new[] { $"{funcName}:{parametersString}" });
        }
        private static Dictionary<string, bool> ReadSolutions()
        {
            Dictionary<string, bool> solvedFunctions = new();

            string[] solutionsFromFile = File.ReadAllLines(@"..\..\..\Files\solutions.txt");

            foreach (string line in solutionsFromFile)
            {
                string key = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] is ':')
                    {
                        solvedFunctions.Add(key, Boolean.Parse(Common.CustomSubstring(line,i + 1)));
                        break;
                    }

                    key += line[i];
                }
            }

            return solvedFunctions;
        }

        private static Dictionary<string, TreeNode> ReadDefinitions()
        {
            Dictionary<string, TreeNode> rootByFuncNames = new();

            string[] definitionsFromFile = File.ReadAllLines(@"..\..\..\Files\definitions.txt");

            foreach (string line in definitionsFromFile)
            {
                string key = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] is ':')
                    {
                        string funcBody = Common.CustomSubstring(line,i + 1);
                        Tree tree = new Tree();
                        TreeNode root = tree.BuildTree(funcBody);
                        rootByFuncNames.Add(key, root);
                        break;
                    }

                    key += line[i];
                }
            }

            return rootByFuncNames;
        }

        private static Dictionary<string, List<string>> ReadParameters()
        {
            Dictionary<string, List<string>> parametersByFuncNames = new();
            string[] parametersFromFile = File.ReadAllLines(@"..\..\..\Files\parameters.txt");

            foreach (string line in parametersFromFile)
            {
                string key = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] is ':')
                    {
                        List<string> currentParameters = new();
                        for (int j = i + 1; j < line.Length; j++)
                        {
                            if (line[j] is not ',')
                            {
                                currentParameters.Add(line[j].ToString());
                            }
                        }

                        parametersByFuncNames.Add(key, currentParameters);
                        break;
                    }

                    key += line[i];
                }
            }

            return parametersByFuncNames;
        }
    }
}
