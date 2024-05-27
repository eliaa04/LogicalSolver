using LogicalSolver;
using LogicalSolver.All;
using LogicalSolver.Common;
using LogicalSolver.Define;
using LogicalSolver.Solve;
using System.Text;

namespace CourseProject
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            CustomDictionary<TreeNode> rootByFuncNames = ReadDefinitions();
            CustomDictionary<List<string>> parametersByFuncNames = ReadParameters();
            CustomDictionary<bool> solvedFunctions = ReadSolutions();


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

                    var commandRemainder = Common.CustomSubstring(command, commandName.Length + 1);
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
                            Define.ValidateDefinitionCandidate(root, parametersList, rootByFuncNames, parametersByFuncNames);
                            rootByFuncNames.Add(funcName, root);
                            parametersByFuncNames.Add(funcName, parametersList);
                            WriteDefinitions(funcName, funcBody);
                            WriteParameters(funcName, parametersList);
                            break;
                        case "SOLVE":
                            if (solvedFunctions.ContainsKey(commandRemainder))
                            {
                                Console.WriteLine(solvedFunctions.Get(commandRemainder));
                                break;
                            }

                            (funcName, currentIndex) = Common.ParseFuncName(commandRemainder);
                            (List<string> argumentsList, currentIndex) = Common.ParseParameters(commandRemainder, currentIndex);

                            TreeNode rootForSolving = null;

                            if (rootByFuncNames.ContainsKey(funcName))
                            {
                                rootForSolving = rootByFuncNames.Get(funcName);
                            }
                            else
                            {
                                throw new Exception($"Функцията {funcName} не е дефинирана");
                            }
                            
                            List<string> parametersForSolving = new();
                            if (parametersByFuncNames.ContainsKey(funcName))
                            {
                                parametersForSolving = parametersByFuncNames.Get(funcName);
                            }

                            Solve.ReplaceParametersWithValues(rootForSolving, parametersForSolving, argumentsList, solvedFunctions,
                                rootByFuncNames);
                            bool treeResult = Solve.SolveNode(rootForSolving);
                            solvedFunctions.Add(commandRemainder, treeResult);
                            WriteSolutions(commandRemainder, treeResult);

                            Console.WriteLine(treeResult);
                            break;
                        case "ALL":
                            if (!rootByFuncNames.ContainsKey(commandRemainder) || !parametersByFuncNames.ContainsKey(commandRemainder))
                            {
                                throw new Exception($"Функцията {commandRemainder} не е дефинирана");
                            }

                            var parameters = parametersByFuncNames.Get(commandRemainder);

                            List<List<string>> variations = new List<List<string>>();

                            var workArr = new List<int>();
                            foreach (var _ in parameters)
                            {
                                workArr.Add(0);
                            }

                            All.GenerateVariationsWithRep(variations, workArr, 2, parameters.Count, 0);

                            string parameterStr = Common.ListToString(string.Empty, parameters);

                            Console.WriteLine($"{parameterStr}:{commandRemainder}->");

                            foreach (var variation in variations)
                            {
                                Solve.ReplaceParametersWithValues(rootByFuncNames.Get(commandRemainder),
                                    parameters,
                                    variation,
                                    solvedFunctions,
                                    rootByFuncNames);

                                bool variationResult = Solve.SolveNode(rootByFuncNames.Get(commandRemainder));

                                string variationStr = Common.ListToString(string.Empty, variation);
                               
                                Console.WriteLine($"{variationStr} :{All.ToNumber(variationResult)}");

                                string argumentsKey = All.ConvertToArgumentsKey(commandRemainder, variation);
                                solvedFunctions.Add(argumentsKey, variationResult);
                                WriteSolutions(argumentsKey, variationResult);
                            }

                            break;
                        default:
                            Console.WriteLine("Командата не е разпозната. Въведете валидна команда!");
                            break;
                    }
                }
                catch (Exception ex)
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
            string parametersString = Common.ListToString(string.Empty, parametersList);
            File.AppendAllLines(@"..\..\..\Files\parameters.txt", new[] { $"{funcName}:{parametersString}" });
        }
        private static CustomDictionary<bool> ReadSolutions()
        {
            CustomDictionary<bool> solvedFunctions = new();

            string[] solutionsFromFile = File.ReadAllLines(@"..\..\..\Files\solutions.txt");

            foreach (string line in solutionsFromFile)
            {
                string key = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] is ':')
                    {
                        solvedFunctions.Add(key, Boolean.Parse(Common.CustomSubstring(line, i + 1)));
                        break;
                    }

                    key += line[i];
                }
            }

            return solvedFunctions;
        }

        private static CustomDictionary<TreeNode> ReadDefinitions()
        {
            CustomDictionary<TreeNode> rootByFuncNames = new();

            string[] definitionsFromFile = File.ReadAllLines(@"..\..\..\Files\definitions.txt");

            foreach (string line in definitionsFromFile)
            {
                string key = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] is ':')
                    {
                        string funcBody = Common.CustomSubstring(line, i + 1);
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

        private static CustomDictionary<List<string>> ReadParameters()
        {
            CustomDictionary<List<string>> parametersByFuncNames = new();
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
