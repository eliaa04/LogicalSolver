using LogicalSolver;
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
            Dictionary<string, TreeNode> rootByFuncNames = new();
            Dictionary<string, List<string>> parametersByFuncNames = new();


            while (true)
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

                var commandRemainder = command.Substring((commandName.Length + 1));
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
                        rootByFuncNames.Add(funcName, root);
                        parametersByFuncNames.Add(funcName, parametersList);
                        break;
                    case "SOLVE":
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

                        Solve.ReplaceParametersWithValues(rootForSolving,parametersForSolving,argumentsList);

                        break;
                    case "ALL":
                        Console.WriteLine("bla bla all");
                        break;
                    default:
                        Console.WriteLine("Командата не е разпозната. Въведете валидна команда!");
                        break;
                }
            }
        }
    }
}
